using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AddSerialSlotCommandTests
    {
        private AddSerialSlotCommand addSerialSlotCommand;

        private Mock<IPremiumAccountManager> premiumAccountManager;
        private Mock<IPremiumCreditsDatabaseRestorePoint> premiumCreditsDatabaseRestorePoint;
        private Mock<IHttpContextReader> httpContextReader;

        private AddSerialSlotRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new AddSerialSlotRequest();

            premiumAccountManager = new Mock<IPremiumAccountManager>();
            premiumCreditsDatabaseRestorePoint = new Mock<IPremiumCreditsDatabaseRestorePoint>();
            httpContextReader = new Mock<IHttpContextReader>();

            premiumAccountManager.Setup(p => p.AddSerialSlot(request)).ReturnsAsync(new AddSerialSlotResult(true, 2));
            premiumCreditsDatabaseRestorePoint.Setup(pcd =>
                    pcd.CreateRestoreParams(It.IsAny<IDatabaseRestoreParams>())
                        .EnqueueToConnectionDatabaseRestorePoints(It.IsAny<string>()))
                .Returns(premiumCreditsDatabaseRestorePoint.Object);

            addSerialSlotCommand = new AddSerialSlotCommand(premiumAccountManager.Object,
                premiumCreditsDatabaseRestorePoint.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_AddingSerialSlotReturnsNull_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.AddSerialSlot(request)).ReturnsAsync(() => null);

            Assert.That(() => addSerialSlotCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void Handle_AddingSerialSlotFailed_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.AddSerialSlot(request)).ReturnsAsync(new AddSerialSlotResult(false, 2));

            Assert.That(() => addSerialSlotCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddSerialSlotResponse()
        {
            var result = await addSerialSlotCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<AddSerialSlotResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}