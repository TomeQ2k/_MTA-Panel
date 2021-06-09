using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetUserSerialsResponse : BaseResponse
    {
        public IEnumerable<SerialDto> UserSerials { get; init; }

        public GetUserSerialsResponse(Error error = null) : base(error)
        {
        }
    }
}