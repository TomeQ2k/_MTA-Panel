using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record ChangeEmailResponse : BaseResponse
    {
        public ChangeEmailResponse(Error error = null) : base(error)
        {
        }
    }
}