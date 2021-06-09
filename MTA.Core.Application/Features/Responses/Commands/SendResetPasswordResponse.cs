using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SendResetPasswordResponse : BaseResponse
    {
        public SendResetPasswordResponse(Error error = null) : base(error)
        {
        }
    }
}