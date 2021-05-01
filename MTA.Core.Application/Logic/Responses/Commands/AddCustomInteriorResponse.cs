using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AddCustomInteriorResponse : BaseResponse
    {
        public AddCustomInteriorResponse(Error error = null) : base(error)
        {
        }
    }
}