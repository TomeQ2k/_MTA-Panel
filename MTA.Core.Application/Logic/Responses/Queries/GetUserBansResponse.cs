using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetUserBansResponse : BaseResponse
    {
        public IEnumerable<PenaltyDto> Bans { get; init; }

        public GetUserBansResponse(Error error = null) : base(error)
        {
        }
    }
}