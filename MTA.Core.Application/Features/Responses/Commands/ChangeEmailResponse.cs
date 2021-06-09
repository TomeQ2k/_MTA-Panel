using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ChangeEmailResponse : BaseResponse
    {
        public ChangeEmailResponse(Error error = null) : base(error)
        {
        }
    }
}