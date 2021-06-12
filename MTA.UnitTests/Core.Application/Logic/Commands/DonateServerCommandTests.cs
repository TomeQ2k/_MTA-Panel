using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Application.SignalR.Hubs;
using MTA.Core.Common.Enums;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class DonateServerCommandTests
    {
        private DonateServerCommand donateServerCommand;

        private Mock<IDonationManager> donationMananger;
        private Mock<IRewardReferrerSystem> rewardReferrerSystem;

        [SetUp]
        public void SetUp()
        {
            donationMananger = new Mock<IDonationManager>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            rewardReferrerSystem = new Mock<IRewardReferrerSystem>();
            var mapper = new Mock<IMapper>();

            donationMananger.Setup(dm => dm.DonateServer(It.IsAny<DonationType>(), It.IsAny<string>()))
                .ReturnsAsync(new DonateServerResult(true, DonationDictionary.CalculateCredits(DonationType.FiftyPLN)));
            donationMananger.Setup(dm => dm.DonateServerDlcBrain(It.IsAny<string>()))
                .ReturnsAsync(new DonateServerResult(true, 0));
            rewardReferrerSystem.Setup(r => r.Reward(RewardReferrerType.ServerDonated, It.IsAny<int>())).ReturnsAsync(
                new RewardReferrerResult(true, It.IsAny<int>(), RewardReferrerType.ServerDonated, It.IsAny<int>()));

            donateServerCommand = new DonateServerCommand(donationMananger.Object, httpContextReader.Object,
                notifier.Object, hubManager.Object, rewardReferrerSystem.Object, mapper.Object);
        }

        [Test]
        public void Handle_DonatingServerFailed_ThrowServerException()
        {
            donationMananger.Setup(dm => dm.DonateServer(It.IsAny<DonationType>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => donateServerCommand.Handle(new DonateServerRequest
            {
                DonationType = DonationType.FiftyPLN
            }, CancellationToken.None), Throws.TypeOf<ServerException>());
        }

        [Test]
        public void Handle_DonatingServerDLCBrainFailed_ThrowServerException()
        {
            donationMananger.Setup(dm => dm.DonateServerDlcBrain(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => donateServerCommand.Handle(new DonateServerRequest
            {
                DonationType = DonationType.ThreeHundredSeventyFivePLN
            }, CancellationToken.None), Throws.TypeOf<ServerException>());
        }

        [Test]
        public void Handle_DonatingServerNotSucceeded_ThrowServerException()
        {
            donationMananger.Setup(dm => dm.DonateServer(It.IsAny<DonationType>(), It.IsAny<string>()))
                .ReturnsAsync(new DonateServerResult(false,
                    DonationDictionary.CalculateCredits(DonationType.FiftyPLN)));

            Assert.That(() => donateServerCommand.Handle(new DonateServerRequest
            {
                DonationType = DonationType.FiftyPLN
            }, CancellationToken.None), Throws.TypeOf<ServerException>());
        }

        [Test]
        public void Handle_WhenCalledWithDonationLessThanThreeHundredSeventyFivePLN_RewardReferrerShouldBeCalled()
        {
            donateServerCommand.Handle(new DonateServerRequest
            {
                DonationType = DonationType.FiftyPLN
            }, CancellationToken.None);

            rewardReferrerSystem.Verify(r => r.Reward(RewardReferrerType.ServerDonated, It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void Handle_WhenCalledWithDonationEqualsThreeHundredSeventyFivePLN_RewardReferrerShouldNotBeCalled()
        {
            donateServerCommand.Handle(new DonateServerRequest
            {
                DonationType = DonationType.ThreeHundredSeventyFivePLN
            }, CancellationToken.None);

            rewardReferrerSystem.Verify(r => r.Reward(RewardReferrerType.ServerDonated, It.IsAny<int>()), Times.Never);
        }

        [Test]
        [TestCase(DonationType.FiftyPLN)]
        [TestCase(DonationType.ThreeHundredSeventyFivePLN)]
        public async Task Handle_WhenCalled_ReturnDonateServerResponse(DonationType donationType)
        {
            var result = await donateServerCommand.Handle(new DonateServerRequest
            {
                DonationType = donationType
            }, CancellationToken.None);

            Assert.That(result, Is.TypeOf<DonateServerResponse>().And.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}