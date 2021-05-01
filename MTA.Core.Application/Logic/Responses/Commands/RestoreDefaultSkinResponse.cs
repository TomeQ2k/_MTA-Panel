using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record RestoreDefaultSkinResponse : BaseResponse
    {
        public RestoreDefaultSkinResponse(Error error = null) : base(error)
        {
        }
    }
}