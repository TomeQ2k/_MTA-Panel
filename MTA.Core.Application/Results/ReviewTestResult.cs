namespace MTA.Core.Application.Results
{
    public record ReviewTestResult
    (
        bool IsSucceeded,
        int UserId
    );
}