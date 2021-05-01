using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AddSerialResponse : BaseResponse
    {
        public AddSerialResponse(Error error = null) : base(error)
        {
        }
    }
}