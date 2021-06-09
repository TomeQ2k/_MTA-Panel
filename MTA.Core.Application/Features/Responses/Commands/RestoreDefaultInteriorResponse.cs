using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record RestoreDefaultInteriorResponse : BaseResponse
    {
        public RestoreDefaultInteriorResponse(Error error = null) : base(error)
        {
        }
    }
}