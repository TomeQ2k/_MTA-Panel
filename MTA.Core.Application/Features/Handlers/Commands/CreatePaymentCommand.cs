using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class CreatePaymentCommand : IRequestHandler<CreatePaymentRequest, CreatePaymentResponse>
    {
        private readonly IPaymentService paymentService;
        private readonly IHttpContextReader httpContextReader;

        public CreatePaymentCommand(IPaymentService paymentService, IHttpContextReader httpContextReader)
        {
            this.paymentService = paymentService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<CreatePaymentResponse> Handle(CreatePaymentRequest request,
            CancellationToken cancellationToken)
        {
            var result = await paymentService.CreatePayment(PaymentUnit.Create(request.Price))
                         ?? throw new PaymentException("Creating payment failed");
            
            return (CreatePaymentResponse) new CreatePaymentResponse
            {
                Result = result
            }.LogInformation(
                $"User #{httpContextReader.CurrentUserId} created payment #{result.OrderId} with price: {request.Price} PLN");
        }
    }
}