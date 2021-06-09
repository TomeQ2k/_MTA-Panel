using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SendResetPasswordByAdminResponse : BaseResponse
    {
        public SendResetPasswordByAdminResponse(Error error = null) : base(error)
        {
        }
    }
}