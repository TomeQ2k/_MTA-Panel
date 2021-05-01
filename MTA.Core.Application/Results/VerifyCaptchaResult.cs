namespace MTA.Core.Application.Results
{
    public record VerifyCaptchaResult
    {
        public bool Success { get; init; }
    }
}