using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data.RestorePoints;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AddCustomInteriorCommandTests
    {
        private AddCustomInteriorCommand addCustomInteriorCommand;

        private Mock<IPremiumAccountManager> premiumAccountManager;
        private Mock<IPremiumCreditsDatabaseRestorePoint> premiumCreditsDatabaseRestorePoint;
        private Mock<IHttpContextReader> httpContextReader;

        private AddCustomInteriorRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new AddCustomInteriorRequest
            {
                InteriorFile = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>()),
                EstateId = 1
            };

            premiumAccountManager = new Mock<IPremiumAccountManager>();
            premiumCreditsDatabaseRestorePoint = new Mock<IPremiumCreditsDatabaseRestorePoint>();
            httpContextReader = new Mock<IHttpContextReader>();

            premiumAccountManager.Setup(p => p.AddCustomInterior(request.InteriorFile, request.EstateId))
                .ReturnsAsync(true);

            addCustomInteriorCommand = new AddCustomInteriorCommand(premiumAccountManager.Object,
                premiumCreditsDatabaseRestorePoint.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_AddingCustomInteriorFailed_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.AddCustomInterior(request.InteriorFile, request.EstateId))
                .ReturnsAsync(false);

            Assert.That(() => addCustomInteriorCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddCustomInteriorResponse()
        {
            var result = await addCustomInteriorCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<AddCustomInteriorResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}