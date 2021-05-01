using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class CapturePaymentCommand : IRequestHandler<CapturePaymentRequest, CapturePaymentResponse>
    {
        private readonly IPaymentService paymentService;
        private readonly IHttpContextReader httpContextReader;

        public CapturePaymentCommand(IPaymentService paymentService, IHttpContextReader httpContextReader)
        {
            this.paymentService = paymentService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<CapturePaymentResponse> Handle(CapturePaymentRequest request,
            CancellationToken cancellationToken)
        {
            var capturePaymentResult = await paymentService.CapturePayment(request.Token) ??
                                       throw new PaymentException(ErrorMessages.PaypalErrorMessage);

            return capturePaymentResult.StatusCode == HttpStatusCode.Created
                ? (CapturePaymentResponse) new CapturePaymentResponse {IsVerified = true}.LogInformation(
                    $"Payment #{request.Token} has been captured by user #{httpContextReader.CurrentUserId}")
                : throw new PaymentException(ErrorMessages.PaypalErrorMessage);
        }
    }
}