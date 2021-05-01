namespace MTA.Core.Application.Results
{
    public record HomepageStatsResult
    {
        public string ServerAddress { get; init; }
        public int ServerPort { get; init; }
        public int OnlinePlayersCount { get; init; }
        public string GameVersion { get; init; }
        public string GameMode { get; init; }
    }
}