using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetCharacterResponse : BaseResponse
    {
        public CharacterDto Character { get; init; }

        public GetCharacterResponse(Error error = null) : base(error)
        {
        }
    }
}