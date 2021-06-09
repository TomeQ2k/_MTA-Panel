using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record DeleteSerialResponse : BaseResponse
    {
        public DeleteSerialResponse(Error error = null) : base(error)
        {
        }
    }
}