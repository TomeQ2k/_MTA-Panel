using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record SetCustomSkinResponse : BaseResponse
    {
        public SetCustomSkinResponse(Error error = null) : base(error)
        {
        }
    }
}