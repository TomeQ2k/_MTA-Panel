using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetCharactersByCharacternameResponse : BaseResponse
    {
        public IEnumerable<CharacterListDto> Characters { get; init; }

        public GetCharactersByCharacternameResponse(Error error = null) : base(error)
        {
        }
    }
}