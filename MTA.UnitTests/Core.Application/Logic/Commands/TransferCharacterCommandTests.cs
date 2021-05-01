using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class TransferCharacterCommandTests
    {
        private TransferCharacterCommand transferCharacterCommand;

        private Mock<IPremiumAccountManager> premiumAccountManager;
        private Mock<IPremiumCreditsDatabaseRestorePoint> premiumCreditsDatabaseRestorePoint;
        private Mock<IHttpContextReader> httpContextReader;

        private TransferCharacterRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new TransferCharacterRequest {SourceCharacterId = 1, TargetCharacterId = 2};

            premiumAccountManager = new Mock<IPremiumAccountManager>();
            premiumCreditsDatabaseRestorePoint = new Mock<IPremiumCreditsDatabaseRestorePoint>();
            httpContextReader = new Mock<IHttpContextReader>();

            premiumAccountManager.Setup(p => p.TransferCharacter(request.SourceCharacterId, request.TargetCharacterId))
                .ReturnsAsync(true);
            premiumCreditsDatabaseRestorePoint.Setup(pcd =>
                    pcd.CreateRestoreParams(It.IsAny<IDatabaseRestoreParams>())
                        .EnqueueToConnectionDatabaseRestorePoints(It.IsAny<string>()))
                .Returns(premiumCreditsDatabaseRestorePoint.Object);

            transferCharacterCommand = new TransferCharacterCommand(premiumAccountManager.Object,
                premiumCreditsDatabaseRestorePoint.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_TransferingCharacterFailed_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.TransferCharacter(request.SourceCharacterId, request.TargetCharacterId))
                .ReturnsAsync(false);

            Assert.That(() => transferCharacterCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnTransferCharacterResponse()
        {
            var result = await transferCharacterCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<TransferCharacterResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}