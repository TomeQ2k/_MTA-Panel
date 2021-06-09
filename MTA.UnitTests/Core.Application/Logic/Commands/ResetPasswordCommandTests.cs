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
    public class ResetPasswordCommandTests
    {
        private Mock<IResetPasswordManager> passwordManager;
        private Mock<ICryptoService> cryptoService;
        private ResetPasswordCommand resetPasswordCommand;

        [SetUp]
        public void SetUp()
        {
            passwordManager = new Mock<IResetPasswordManager>();
            cryptoService = new Mock<ICryptoService>();

            cryptoService.Setup(cs => cs.Decrypt(It.IsAny<string>()))
                .Returns("test");

            resetPasswordCommand = new(passwordManager.Object, cryptoService.Object);
        }

        [Test]
        public void Handle_ResetPasswordFailed_ThrowResetPasswordException()
        {
            var request = new ResetPasswordRequest();

            passwordManager.Setup(pm => pm.ResetPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            Assert.That(() => resetPasswordCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<ResetPasswordException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnResetPasswordResponse()
        {
            var request = new ResetPasswordRequest
            {
                Token = "test",
                Email = "test",
                NewPassword = "test"
            };

            passwordManager.Setup(pm => pm.ResetPassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var response = await resetPasswordCommand.Handle(request, It.IsAny<CancellationToken>());
            Assert.That(response, Is.TypeOf<ResetPasswordResponse>());
        }
    }
}