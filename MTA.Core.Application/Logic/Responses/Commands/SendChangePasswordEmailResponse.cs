using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record SendChangePasswordEmailResponse : BaseResponse
    {
        public SendChangePasswordEmailResponse(Error error = null) : base(error)
        {
        }
    }
}