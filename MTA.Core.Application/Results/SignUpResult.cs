using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Results
{
    public record SignUpResult
    {
        public string TokenCode { get; init; }
        public User User { get; init; }
    }
}