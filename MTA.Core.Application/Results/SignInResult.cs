using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Results
{
    public record SignInResult
    {
        public User User { get; init; }
        public string JwtToken { get; init; }
    }
}