namespace MTA.Core.Application.Results
{
    public record TogglePrivacyReportResult
    (
        bool IsPrivate,
        bool IsSucceeded
    );
}