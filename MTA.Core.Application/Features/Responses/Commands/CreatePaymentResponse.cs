using MTA.Core.Application.Models;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record CreatePaymentResponse : BaseResponse
    {
        public PaymentResult Result { get; init; }

        public CreatePaymentResponse(Error error = null) : base(error)
        {
        }
    }
}