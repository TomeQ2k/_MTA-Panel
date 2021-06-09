using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Features.Handlers.Queries;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Queries
{
    [TestFixture]
    public class VerifyResetPasswordQueryTests
    {
        private Mock<IResetPasswordManager> resetPasswordManager;
        private Mock<ICryptoService> cryptoService;
        private VerifyResetPasswordRequest request;
        private VerifyResetPasswordQuery verifyResetPasswordQuery;

        [SetUp]
        public void SetUp()
        {
            resetPasswordManager = new Mock<IResetPasswordManager>();
            cryptoService = new Mock<ICryptoService>();
            request = new VerifyResetPasswordRequest();

            verifyResetPasswordQuery = new VerifyResetPasswordQuery(resetPasswordManager.Object, cryptoService.Object);
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnVerifyResetPasswordResponse()
        {
            resetPasswordManager.Setup(r => r.VerifyResetPasswordToken(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var result = await verifyResetPasswordQuery.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<VerifyResetPasswordResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}