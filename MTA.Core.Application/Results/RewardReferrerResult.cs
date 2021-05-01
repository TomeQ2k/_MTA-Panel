using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Results
{
    public record RewardReferrerResult
    (
        bool IsRewarded,
        int RewardCredits,
        RewardReferrerType RewardType,
        int ReferrerId
    );
}