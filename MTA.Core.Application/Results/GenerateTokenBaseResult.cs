namespace MTA.Core.Application.Results
{
    public abstract record GenerateTokenBaseResult
    {
        public string Email { get; init; }
        public string Token { get; init; }
        public string Username { get; init; }
    }
}