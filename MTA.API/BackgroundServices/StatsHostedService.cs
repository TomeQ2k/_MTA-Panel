using System;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.BackgroundServices
{
    internal class StatsHostedService : ServerHostedService
    {
        public StatsHostedService(IServiceProvider services) : base(services)
        {
            TimeInterval = Constants.StatsHostedServiceTimeInMinutes;
        }

        public override async void Callback(object state)
        {
            using (var scope = services.CreateScope())
            {
                var (serverStatsService, playersActivityStatsService, factionsStatsService, adminsActivityStatsService,
                    moneyStatsService) = FindStatsServices(scope);

                var filesManager = scope.ServiceProvider.GetRequiredService<IFilesManager>();
                var statsMemoryCacheService = scope.ServiceProvider.GetRequiredService<IStatsMemoryCacheService>();

                var dataPath = $"{filesManager.WebRootPath}/data/stats.json";

                var (serverStats, playersActivityStats, factionsStats, adminsActivityStats, moneyStats) =
                    (await serverStatsService.SelectStats(), await playersActivityStatsService.SelectStats(),
                        await factionsStatsService.SelectStats(),
                        await adminsActivityStatsService.SelectStats(), await moneyStatsService.SelectStats());

                var statsJson = new {serverStats, playersActivityStats, factionsStats, adminsActivityStats, moneyStats}
                    .ToJSON();

                await filesManager.WriteFile(statsJson, dataPath);

                statsMemoryCacheService.Set(MemoryCacheKeys.STATS_KEY,
                    statsJson.FromJSON<StatsModel>(JsonSettings.JsonSerializerOptions),
                    new MemoryCacheEntryOptions().SetSlidingExpiration(
                        TimeSpan.FromMinutes(Constants.StatsHostedServiceTimeInMinutes)));

                Log.Information("MTA stats has been updated");
                base.Callback(state);
            }
        }

        #region private

        private static (IStatsService<ServerStatsResult>, IStatsService<PlayersActivityStatsResult>,
            IStatsService<FactionsStatsResult>, IStatsService<AdminsActivityStatsResult>,
            IStatsService<MoneyStatsResult>) FindStatsServices(IServiceScope scope)
            => (scope.ServiceProvider
                .GetRequiredService<IStatsService<ServerStatsResult>>(), scope.ServiceProvider
                .GetRequiredService<IStatsService<PlayersActivityStatsResult>>(), scope.ServiceProvider
                .GetRequiredService<IStatsService<FactionsStatsResult>>(), scope.ServiceProvider
                .GetRequiredService<IStatsService<AdminsActivityStatsResult>>(), scope.ServiceProvider
                .GetRequiredService<IStatsService<MoneyStatsResult>>());

        #endregion
    }
}