using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record AddCustomSkinResponse : BaseResponse
    {
        public AddCustomSkinResponse(Error error = null) : base(error)
        {
        }
    }
}