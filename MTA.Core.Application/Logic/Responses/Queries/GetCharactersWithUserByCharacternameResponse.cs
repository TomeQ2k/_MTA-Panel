using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetCharactersWithUserByCharacternameResponse : BaseResponse
    {
        public IEnumerable<CharacterWithUserDto> CharactersWithUser { get; init; }

        public GetCharactersWithUserByCharacternameResponse(Error error = null) : base(error)
        {
        }
    }
}