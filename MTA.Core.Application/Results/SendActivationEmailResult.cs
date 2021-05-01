namespace MTA.Core.Application.Results
{
    public record SendActivationEmailResult
    {
        public string Email { get; init; }
        public string Token { get; init; }
        public string Username { get; init; }
    }
}