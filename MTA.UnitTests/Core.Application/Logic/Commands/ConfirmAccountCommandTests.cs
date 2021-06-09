using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class ConfirmAccountCommandTests
    {
        private Mock<IAuthService> authService;
        private Mock<ICryptoService> cryptoService;
        private ConfirmAccountRequest request;
        private ConfirmAccountCommand confirmAccountCommand;

        [SetUp]
        public void SetUp()
        {
            authService = new Mock<IAuthService>();
            cryptoService = new Mock<ICryptoService>();
            request = new ConfirmAccountRequest();

            confirmAccountCommand = new ConfirmAccountCommand(authService.Object, cryptoService.Object);
        }

        [Test]
        public void Handle_ConfirmingAccountFailed_ThrowAccountNotConfirmedException()
        {
            authService.Setup(a => a.ConfirmAccount(It.IsNotNull<string>(), It.IsNotNull<string>()))
                .ReturnsAsync(false);

            Assert.That(() => confirmAccountCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.Exception.TypeOf<AccountNotConfirmedException>());
        }

        [Test]
        public async Task Handle_AccountConfirmed_ReturnConfirmAccountResponse()
        {
            authService.Setup(a => a.ConfirmAccount(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await confirmAccountCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ConfirmAccountResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}