using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record FetchStatsResponse : BaseResponse
    {
        public StatsModel Stats { get; init; }

        public FetchStatsResponse(Error error = null) : base(error)
        {
        }
    }
}