using Ardalis.SmartEnum;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.SmartEnums
{
    public abstract class RewardReferrerTypeSmartEnum : SmartEnum<RewardReferrerTypeSmartEnum>
    {
        protected RewardReferrerTypeSmartEnum(string name, int value) : base(name, value)
        {
        }

        public static RewardReferrerTypeSmartEnum ServerDonated = new ServerDonatedType();

        public abstract int CalculateRewardCredits(int baseRewardCredits);

        private sealed class ServerDonatedType : RewardReferrerTypeSmartEnum
        {
            public ServerDonatedType() : base(nameof(ServerDonated), (int) RewardReferrerType.ServerDonated)
            {
            }

            public override int CalculateRewardCredits(int baseRewardCredits) => (int) (baseRewardCredits * 0.1);
        }
    }
}