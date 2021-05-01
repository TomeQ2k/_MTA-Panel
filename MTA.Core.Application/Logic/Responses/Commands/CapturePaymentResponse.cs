using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record CapturePaymentResponse : BaseResponse
    {
        public bool IsVerified { get; init; }

        public CapturePaymentResponse(Error error = null) : base(error)
        {
        }
    }
}