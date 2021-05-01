using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record SendAddSerialEmailResponse : BaseResponse
    {
        public SendAddSerialEmailResponse(Error error = null) : base(error)
        {
        }
    }
}