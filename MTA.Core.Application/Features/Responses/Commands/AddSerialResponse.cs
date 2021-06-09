using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record AddSerialResponse : BaseResponse
    {
        public AddSerialResponse(Error error = null) : base(error)
        {
        }
    }
}