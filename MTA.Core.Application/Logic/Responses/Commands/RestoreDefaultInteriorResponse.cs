using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record RestoreDefaultInteriorResponse : BaseResponse
    {
        public RestoreDefaultInteriorResponse(Error error = null) : base(error)
        {
        }
    }
}