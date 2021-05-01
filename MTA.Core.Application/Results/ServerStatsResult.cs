namespace MTA.Core.Application.Results
{
    public record ServerStatsResult
    {
        public int AccountsCount { get; init; }
        public int CharactersCount { get; init; }
        public int EstatesCount { get; init; }
        public int VehiclesCount { get; init; }
        public int HoursPlayedCount { get; init; }
        public long BankTotalMoney { get; init; }
    }
}