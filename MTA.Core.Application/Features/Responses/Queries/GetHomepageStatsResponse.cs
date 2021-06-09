using MTA.Core.Application.Models;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetHomepageStatsResponse : BaseResponse
    {
        public HomepageStatsResult HomepageStats { get; init; }

        public GetHomepageStatsResponse(Error error = null) : base(error)
        {
        }
    }
}