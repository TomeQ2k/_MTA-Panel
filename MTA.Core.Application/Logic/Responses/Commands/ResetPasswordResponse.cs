using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record ResetPasswordResponse : BaseResponse
    {
        public ResetPasswordResponse(Error error = null) : base(error)
        {
        }
    }
}