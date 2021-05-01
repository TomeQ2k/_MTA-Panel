using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Factories.Interfaces;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Settings;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using Token = MTA.Core.Domain.Entities.Token;

namespace MTA.Infrastructure.Shared.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaypalSettings paypalSettings;
        private readonly IOrderTransactionService orderTransactionService;
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;
        private readonly PayPalHttpClient paypalClient;

        public PaymentService(IOptions<PaypalSettings> paypalSettings, IPaypalClientFactory paypalClientFactory,
            IOrderTransactionService orderTransactionService, IDatabase database, IHttpContextReader httpContextReader)
        {
            this.paypalSettings = paypalSettings.Value;
            this.orderTransactionService = orderTransactionService;
            this.database = database;
            this.httpContextReader = httpContextReader;

            this.paypalClient = paypalClientFactory.CreateClient(this.paypalSettings);
        }

        public async Task<PaymentResult> CreatePayment(params PaymentUnit[] paymentUnits)
        {
            var orderRequest = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>(paymentUnits.Select(pu => new PurchaseUnitRequest
                {
                    AmountWithBreakdown = pu.Amount
                })),
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = paypalSettings.ReturnUrl,
                    CancelUrl = paypalSettings.CancelUrl
                }
            };

            var orderCreateRequest = new OrdersCreateRequest()
                .Prefer("return=representation")
                .RequestBody(orderRequest);

            var response = await paypalClient.Execute(orderCreateRequest)
                           ?? throw new PaymentException(ErrorMessages.PaypalErrorMessage);

            var order = response.Result<Order>();

            var paymentToken = Token.Create(TokenType.Payment, httpContextReader.CurrentUserId).SetCode(order.Id);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (!await database.TokenRepository.Insert(paymentToken, false))
                    throw new DatabaseException();

                if (await orderTransactionService.CreateOrderTransaction(order.Id,
                    decimal.Parse(paymentUnits.FirstOrDefault()?.Amount.Value ?? string.Empty),
                    new EmailUsernameTuple(httpContextReader.CurrentEmail, httpContextReader.CurrentUsername)) == null)
                    throw new DatabaseException();

                transaction.Complete();
            }

            return new PaymentResult
            {
                OrderId = order.ExpirationTime,
                Links = order.Links.Select(l => new OrderLink(l.Href, l.Rel)),
                StatusCode = response.StatusCode
            };
        }

        public async Task<PaymentResult> CapturePayment(string orderId)
        {
            var token = await database.TokenRepository.GetTokenByCodeAndType(orderId, TokenType.Payment,
                            httpContextReader.CurrentUserId)
                        ?? throw new EntityNotFoundException("Token not found");

            if (token.DateCreated < DateTime.Now.AddHours(-Constants.PaymentTokenExpireTimeInHours))
                throw new TokenExpiredException();

            var orderCaptureRequest = new OrdersCaptureRequest(orderId)
                .RequestBody(new OrderActionRequest());

            var orderTransaction = await database.OrderTransactionRepository.FindByColumn(
                new("transaction_id", orderId)) ?? throw new DatabaseException();

            orderTransaction.Validate();

            if (!await database.OrderTransactionRepository.Update(orderTransaction))
                throw new DatabaseException();

            var response = await paypalClient.Execute(orderCaptureRequest)
                           ?? throw new PaymentException(ErrorMessages.PaypalErrorMessage);

            return new PaymentResult
            {
                OrderId = response.Result<Order>().Id,
                StatusCode = response.StatusCode
            };
        }
    }
}