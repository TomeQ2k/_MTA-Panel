using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class RewardReferrerSystemTests
    {
        private RewardReferrerSystem rewardReferrerSystem;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;

        private User user;
        private User referrer;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            user.SetReferrer(2);

            referrer = new User();

            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.UserRepository.FindById(UserId)).ReturnsAsync(user);
            database.Setup(d => d.UserRepository.FindById(user.ReferrerId)).ReturnsAsync(referrer);
            database.Setup(d => d.UserRepository.Update(referrer)).ReturnsAsync(true);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            rewardReferrerSystem = new RewardReferrerSystem(database.Object, httpContextReader.Object);
        }

        #region Reward

        [Test]
        public void Reward_UserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(UserId)).ReturnsAsync(() => null);

            Assert.That(() => rewardReferrerSystem.Reward(RewardReferrerType.ServerDonated, 0),
                Throws.Exception.TypeOf<AuthException>());
        }

        [Test]
        public async Task Reward_UserHasNotReferrer_ReturnFalseRewardReferrerResult()
        {
            user.SetReferrer(default);

            var result = await rewardReferrerSystem.Reward(RewardReferrerType.ServerDonated, 0);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<RewardReferrerResult>());
            Assert.That(result.IsRewarded, Is.False);
            Assert.That(result.ReferrerId, Is.EqualTo(user.ReferrerId));
        }

        [Test]
        public async Task Reward_ReferrerNotFound_ReturnFalseRewardReferrerResult()
        {
            database.Setup(d => d.UserRepository.FindById(user.ReferrerId)).ReturnsAsync(() => null);

            var result = await rewardReferrerSystem.Reward(RewardReferrerType.ServerDonated, 0);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<RewardReferrerResult>());
            Assert.That(result.IsRewarded, Is.False);
            Assert.That(result.ReferrerId, Is.EqualTo(user.ReferrerId));
        }

        [Test]
        public async Task Reward_UpdateReferrerInDatabaseFailed_ReturnFalseRewardReferrerResult()
        {
            database.Setup(d => d.UserRepository.Update(referrer)).ReturnsAsync(false);

            var result = await rewardReferrerSystem.Reward(RewardReferrerType.ServerDonated, 0);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<RewardReferrerResult>());
            Assert.That(result.IsRewarded, Is.False);
            Assert.That(result.ReferrerId, Is.EqualTo(user.ReferrerId));
        }

        [Test]
        public async Task Reward_WhenCalled_ReturnTrueRewardReferrerResult()
        {
            var result = await rewardReferrerSystem.Reward(RewardReferrerType.ServerDonated, 0);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<RewardReferrerResult>());
            Assert.That(result.IsRewarded, Is.True);
            Assert.That(result.ReferrerId, Is.EqualTo(user.ReferrerId));
        }

        #endregion
    }
}