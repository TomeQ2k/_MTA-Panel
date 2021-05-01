using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class RewardReferrerSystem : IRewardReferrerSystem
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public RewardReferrerSystem(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<RewardReferrerResult> Reward(RewardReferrerType rewardType, int baseRewardCredits = 0)
        {
            var user = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                       throw new AuthException("User is not authenticated");

            if (user.ReferrerId == default)
                return new RewardReferrerResult(false, default, rewardType, user.ReferrerId);

            var referrer = await database.UserRepository.FindById(user.ReferrerId);

            if (referrer == null)
                return new RewardReferrerResult(false, default, rewardType, user.ReferrerId);

            int rewardCredits = RewardReferrerTypeSmartEnum.FromValue((int) rewardType)
                .CalculateRewardCredits(baseRewardCredits);

            referrer.AddCredits(rewardCredits);

            return await database.UserRepository.Update(referrer)
                ? new RewardReferrerResult(true, rewardCredits, rewardType, user.ReferrerId)
                : new RewardReferrerResult(false, rewardCredits, rewardType, user.ReferrerId);
        }
    }
}