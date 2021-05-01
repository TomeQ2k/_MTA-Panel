using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AddObjectProtectionCommandTests
    {
        private AddObjectProtectionCommand addObjectProtectionCommand;

        private Mock<IPremiumAccountManager> premiumAccountManager;
        private Mock<IPremiumCreditsDatabaseRestorePoint> premiumCreditsDatabaseRestorePoint;
        private Mock<IHttpContextReader> httpContextReader;

        private AddObjectProtectionRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new AddObjectProtectionRequest {ObjectId = 1, ProtectionType = ObjectProtectionType.Estate};

            premiumAccountManager = new Mock<IPremiumAccountManager>();
            premiumCreditsDatabaseRestorePoint = new Mock<IPremiumCreditsDatabaseRestorePoint>();
            httpContextReader = new Mock<IHttpContextReader>();

            premiumAccountManager.Setup(p => p.AddObjectProtection(request))
                .ReturnsAsync(new ObjectProtectionResult(true, request.ProtectionType, request.ObjectId));
            premiumCreditsDatabaseRestorePoint.Setup(pcd =>
                    pcd.CreateRestoreParams(It.IsAny<IDatabaseRestoreParams>())
                        .EnqueueToConnectionDatabaseRestorePoints(It.IsAny<string>()))
                .Returns(premiumCreditsDatabaseRestorePoint.Object);

            addObjectProtectionCommand = new AddObjectProtectionCommand(premiumAccountManager.Object,
                premiumCreditsDatabaseRestorePoint.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_AddingObjectProtectionReturnsNull_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.AddObjectProtection(request))
                .ReturnsAsync(() => null);

            Assert.That(() => addObjectProtectionCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void Handle_AddingObjectProtectionFailed_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.AddObjectProtection(request))
                .ReturnsAsync(new ObjectProtectionResult(false, request.ProtectionType, request.ObjectId));

            Assert.That(() => addObjectProtectionCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddObjectProtectionResponse()
        {
            var result = await addObjectProtectionCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<AddObjectProtectionResponse>());
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result.ProtectionType, Is.EqualTo(request.ProtectionType));
            Assert.That(result.ObjectId, Is.EqualTo(request.ObjectId));
        }
    }
}