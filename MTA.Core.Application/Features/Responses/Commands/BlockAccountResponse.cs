using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record BlockAccountResponse : BaseResponse
    {
        public BlockAccountResponse(Error error = null) : base(error)
        {
        }
    }
}