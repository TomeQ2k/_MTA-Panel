using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record TransferCharacterResponse : BaseResponse
    {
        public TransferCharacterResponse(Error error = null) : base(error)
        {
        }
    }
}