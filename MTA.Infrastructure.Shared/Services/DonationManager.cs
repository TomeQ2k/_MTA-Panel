using System;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class DonationManager : IDonationManager
    {
        private readonly IDatabase database;
        private readonly IUserManager userManager;
        private readonly IHttpContextReader httpContextReader;

        public DonationManager(IDatabase database, IUserManager userManager, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.userManager = userManager;
            this.httpContextReader = httpContextReader;
        }

        public async Task<DonateServerResult> DonateServer(DonationType donationType, string code)
        {
            var token = await VerifyAndGetPaymentToken(code);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                var _ = await userManager.AddCredits(DonationDictionary.CalculateCredits(donationType),
                    httpContextReader.CurrentUserId) ?? throw new DatabaseException();

                await ExecuteOrderTransaction(donationType, code, token);

                transaction.Complete();

                return new DonateServerResult(true, DonationDictionary.CalculateCredits(donationType));
            }
        }

        public async Task<DonateServerResult> DonateServerDlcBrain(string code)
        {
            var token = await VerifyAndGetPaymentToken(code);

            using (var transaction = database.BeginTransaction().Transaction)
            {
                var report = new ReportBuilder()
                    .SetSubject(Constants.DonationReportSubject(token.Code))
                    .SetContent(Constants.DonationReportContent(httpContextReader.CurrentUserId,
                        httpContextReader.CurrentUsername))
                    .SetCategoryType(ReportCategoryType.Donation)
                    .CreatedBy(httpContextReader.CurrentUserId)
                    .Build();

                if (!await database.ReportRepository.Insert(report, false))
                    throw new DatabaseException();

                await ExecuteOrderTransaction(DonationType.ThreeHundredSeventyFivePLN, code, token);

                transaction.Complete();

                return new DonateServerResult(true, 0);
            }
        }

        #region private

        private async Task<Token> VerifyAndGetPaymentToken(string code)
        {
            var token = await database.TokenRepository.GetTokenByCodeAndType(code, TokenType.Payment,
                httpContextReader.CurrentUserId) ?? throw new EntityNotFoundException("Token not found");

            if (token.DateCreated.AddHours(Constants.PaymentTokenExpireTimeInHours) < DateTime.Now)
                throw new TokenExpiredException();

            return token;
        }

        private async Task ExecuteOrderTransaction(DonationType donationType, string code, Token token)
        {
            var orderTransaction = await database.OrderTransactionRepository.Find(new SqlBuilder()
                .Append("transaction_id").Equals.Append($"'{code}'")
                .And
                .Append("amount").Equals.Append((decimal) donationType)
                .Build().Query) ?? throw new ServerException("Donating server failed");

            if (!orderTransaction.IsValidated)
                throw new NoPermissionsException("Order transaction is not validated");

            if (!await database.TokenRepository.Delete(token))
                throw new DatabaseException();
        }

        #endregion
    }
}