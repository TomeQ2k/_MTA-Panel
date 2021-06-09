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
    public class AddCustomSkinCommandTests
    {
        private AddCustomSkinCommand addCustomSkinCommand;

        private Mock<IPremiumAccountManager> premiumAccountManager;
        private Mock<IPremiumCreditsDatabaseRestorePoint> premiumCreditsDatabaseRestorePoint;
        private Mock<IHttpContextReader> httpContextReader;

        private AddCustomSkinRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new AddCustomSkinRequest
            {
                CharacterId = 1, SkinFile = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>()),
                SkinId = 1
            };

            premiumAccountManager = new Mock<IPremiumAccountManager>();
            premiumCreditsDatabaseRestorePoint = new Mock<IPremiumCreditsDatabaseRestorePoint>();
            httpContextReader = new Mock<IHttpContextReader>();

            premiumAccountManager.Setup(p => p.AddCustomSkin(request.SkinFile, request.SkinId, request.CharacterId))
                .ReturnsAsync(true);

            addCustomSkinCommand = new AddCustomSkinCommand(premiumAccountManager.Object,
                premiumCreditsDatabaseRestorePoint.Object, httpContextReader.Object);
        }

        [Test]
        public void Handle_AddingCustomSkinFailed_ThrowPremiumOperationException()
        {
            premiumAccountManager.Setup(p => p.AddCustomSkin(request.SkinFile, request.SkinId, request.CharacterId))
                .ReturnsAsync(false);

            Assert.That(() => addCustomSkinCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<PremiumOperationException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddCustomSkinResponse()
        {
            var result = await addCustomSkinCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<AddCustomSkinResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}