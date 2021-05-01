using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record SetCustomInteriorResponse : BaseResponse
    {
        public SetCustomInteriorResponse(Error error = null) : base(error)
        {
        }
    }
}