using System.Collections.Generic;
using MTA.Core.Application.Exceptions;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Helpers
{
    public static class DonationDictionary
    {
        private static Dictionary<DonationType, int> donations => new Dictionary<DonationType, int>
        {
            {DonationType.ThreePLN, 3},
            {DonationType.FivePLN, 5},
            {DonationType.TenPLN, 11},
            {DonationType.TwentyPLN, 24},
            {DonationType.FiftyPLN, 75},
            {DonationType.OneHundredTenPLN, 155},
            {DonationType.OneHundredSeventyFivePLN, 247},
            {DonationType.ThreeHundredPLN, 423}
        };

        public static int CalculateCredits(DonationType donationType) => donations.ContainsKey(donationType)
            ? donations[donationType]
            : throw new ServerException("Such donation does not exist");
    }
}