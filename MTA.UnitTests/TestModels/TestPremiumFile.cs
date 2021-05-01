using MTA.Core.Domain.Entities;

namespace MTA.UnitTests.TestModels
{
    public class TestPremiumFile : PremiumFile
    {
        public void SetDateCreated(int days) => DateCreated = DateCreated.AddDays(days);
    }
}