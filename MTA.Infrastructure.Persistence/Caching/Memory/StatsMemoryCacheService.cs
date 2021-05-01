using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MTA.Core.Application.Caching;
using MTA.Core.Application.Caching.Memory;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Application.Settings;
using MTA.Core.Common.Helpers;

namespace MTA.Infrastructure.Persistence.Caching.Memory
{
    public class StatsMemoryCacheService : MemoryCacheService<StatsModel>, IStatsMemoryCacheService
    {
        private readonly IReadOnlyFilesManager filesManager;

        private static TimeSpan statsCacheSlidingExpiration =
            TimeSpan.FromMinutes(Constants.StatsHostedServiceTimeInMinutes);

        public StatsMemoryCacheService(IMemoryCache memoryCache, IReadOnlyFilesManager filesManager) : base(memoryCache)
        {
            this.filesManager = filesManager;
        }

        public override async Task<StatsModel> Get(string key)
        {
            var statsModel = await base.Get(key);

            if (statsModel != null)
                return statsModel;

            statsModel = (await filesManager.ReadFile($"{filesManager.WebRootPath}/data/stats.json"))?
                .FromJSON<StatsModel>(JsonSettings.JsonSerializerOptions);

            this.Set(MemoryCacheKeys.STATS_KEY, statsModel, new MemoryCacheEntryOptions()
                .SetSlidingExpiration(statsCacheSlidingExpiration));

            return statsModel;
        }
    }
}