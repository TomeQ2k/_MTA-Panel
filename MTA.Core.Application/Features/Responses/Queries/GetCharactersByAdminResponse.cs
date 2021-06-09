using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetCharactersByAdminResponse : BaseResponse
    {
        public IEnumerable<CharacterAdminListDto> Characters { get; init; }

        public GetCharactersByAdminResponse(Error error = null) : base(error)
        {
        }
    }
}