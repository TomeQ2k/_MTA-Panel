namespace MTA.Core.Application.Results
{
    public record SerialExistsResult
    {
        public bool SerialExists { get; init; }
    }
}