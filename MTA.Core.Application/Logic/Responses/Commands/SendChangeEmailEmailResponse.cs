using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record SendChangeEmailEmailResponse : BaseResponse
    {
        public SendChangeEmailEmailResponse(Error error = null) : base(error)
        {
        }
    }
}