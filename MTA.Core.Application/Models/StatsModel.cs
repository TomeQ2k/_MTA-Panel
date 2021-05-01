using MTA.Core.Application.Results;

namespace MTA.Core.Application.Models
{
    public record StatsModel
    {
        public ServerStatsResult ServerStats { get; init; }
        public PlayersActivityStatsResult PlayersActivityStats { get; init; }
        public FactionsStatsResult FactionsStats { get; init; }
        public AdminsActivityStatsResult AdminsActivityStats { get; init; }
        public MoneyStatsResult MoneyStats { get; init; }
    }
}