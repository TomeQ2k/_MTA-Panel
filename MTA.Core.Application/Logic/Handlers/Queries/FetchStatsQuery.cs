using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Caching;
using MTA.Core.Application.Caching.Memory;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class FetchStatsQuery : IRequestHandler<FetchStatsRequest, FetchStatsResponse>
    {
        private readonly IStatsMemoryCacheService statsMemoryCacheService;

        public FetchStatsQuery(IStatsMemoryCacheService statsMemoryCacheService)
        {
            this.statsMemoryCacheService = statsMemoryCacheService;
        }

        public async Task<FetchStatsResponse> Handle(FetchStatsRequest request, CancellationToken cancellationToken)
            => new FetchStatsResponse {Stats = await statsMemoryCacheService.Get(MemoryCacheKeys.STATS_KEY)};
    }
}