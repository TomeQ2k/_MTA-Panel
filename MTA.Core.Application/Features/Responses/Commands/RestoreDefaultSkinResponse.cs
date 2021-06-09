using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record RestoreDefaultSkinResponse : BaseResponse
    {
        public RestoreDefaultSkinResponse(Error error = null) : base(error)
        {
        }
    }
}