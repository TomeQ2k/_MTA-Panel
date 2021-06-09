using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetChangelogsResponse : BaseResponse
    {
        public IEnumerable<ChangelogDto> Changelogs { get; init; }

        public GetChangelogsResponse(Error error = null) : base(error)
        {
        }
    }
}