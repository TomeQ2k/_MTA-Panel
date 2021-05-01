using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record SendResetPasswordByAdminResponse : BaseResponse
    {
        public SendResetPasswordByAdminResponse(Error error = null) : base(error)
        {
        }
    }
}