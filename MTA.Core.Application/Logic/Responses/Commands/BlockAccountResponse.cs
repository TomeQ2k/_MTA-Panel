using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record BlockAccountResponse : BaseResponse
    {
        public BlockAccountResponse(Error error = null) : base(error)
        {
        }
    }
}