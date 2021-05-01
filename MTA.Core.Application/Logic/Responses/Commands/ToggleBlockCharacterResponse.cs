using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record ToggleBlockCharacterResponse : BaseResponse
    {
        public bool IsBlocked { get; init; }

        public ToggleBlockCharacterResponse(Error error = null) : base(error)
        {
        }
    }
}