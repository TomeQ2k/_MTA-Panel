using System.Threading.Tasks;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Services
{
    public interface IRewardReferrerSystem
    {
        Task<RewardReferrerResult> Reward(RewardReferrerType rewardType, int baseRewardCredits = 0);
    }
}