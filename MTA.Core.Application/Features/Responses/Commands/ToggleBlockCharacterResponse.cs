using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record ToggleBlockCharacterResponse : BaseResponse
    {
        public bool IsBlocked { get; init; }

        public ToggleBlockCharacterResponse(Error error = null) : base(error)
        {
        }
    }
}