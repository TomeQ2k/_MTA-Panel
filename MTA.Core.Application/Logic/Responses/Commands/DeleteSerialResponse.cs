using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record DeleteSerialResponse : BaseResponse
    {
        public DeleteSerialResponse(Error error = null) : base(error)
        {
        }
    }
}