namespace MTA.Core.Domain.Data.RestorePoints.Params
{
    public record PremiumCreditsDatabaseRestoreParams
    (
        int CreditsToRefund,
        int UserId
    ) : IDatabaseRestoreParams;
}