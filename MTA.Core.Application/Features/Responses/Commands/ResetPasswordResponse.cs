using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ResetPasswordResponse : BaseResponse
    {
        public ResetPasswordResponse(Error error = null) : base(error)
        {
        }
    }
}