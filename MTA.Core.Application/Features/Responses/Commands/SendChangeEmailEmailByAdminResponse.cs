using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SendChangeEmailEmailByAdminResponse : BaseResponse
    {
        public SendChangeEmailEmailByAdminResponse(Error error = null) : base(error)
        {
        }
    }
}