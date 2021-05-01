namespace MTA.Core.Application.Results
{
    public record DonateServerResult
    (
        bool IsSucceeded,
        int CreditsAdded
    );
}