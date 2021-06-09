using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SendChangePasswordEmailResponse : BaseResponse
    {
        public SendChangePasswordEmailResponse(Error error = null) : base(error)
        {
        }
    }
}