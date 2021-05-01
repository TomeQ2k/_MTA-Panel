using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class SendChangeEmailEmailByAdminCommandTests
    {
        private SendChangeEmailEmailByAdminRequest request;
        private Mock<IUserTokenGenerator> userTokenGenerator;
        private Mock<IEmailSender> emailSender;
        private Mock<IAuthValidationService> authValidationService;
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;
        private Mock<ICryptoService> cryptoService;
        private SendChangeEmailEmailByAdminCommand sendChangeEmailEmailByAdminCommand;

        private const string Test = "test";

        [SetUp]
        public void SetUp()
        {
            var generateChangeEmailTokenResult = new GenerateChangeEmailTokenResult
            {
                Email = Test,
                Token = Test,
                Username = Test
            };

            var emailTemplate = new EmailTemplate
            {
                TemplateName = Test,
                Subject = Test,
                Content = Test,
                AllowedParameters = new[] {"{{username}}", "{{callbackUrl}}"}
            };

            request = new SendChangeEmailEmailByAdminRequest
            {
                UserId = 1,
                NewEmail = Test
            };

            userTokenGenerator = new Mock<IUserTokenGenerator>();
            authValidationService = new Mock<IAuthValidationService>();
            emailSender = new Mock<IEmailSender>();
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();
            cryptoService = new Mock<ICryptoService>();
            var configuration = new Mock<IConfiguration>();

            authValidationService.Setup(avs => avs.EmailExists(It.IsAny<string>())).ReturnsAsync(false);
            userTokenGenerator.Setup(c => c.GenerateChangeEmailTokenByAdmin(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(generateChangeEmailTokenResult);
            cryptoService.Setup(c => c.Encrypt(It.IsAny<string>()))
                .Returns(Test);
            configuration.Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);
            emailTemplateGenerator.Setup(etg => etg.FindEmailTemplate(It.IsAny<string>()))
                .ReturnsAsync(emailTemplate);
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(true);

            sendChangeEmailEmailByAdminCommand = new SendChangeEmailEmailByAdminCommand(userTokenGenerator.Object,
                authValidationService.Object, emailSender.Object, emailTemplateGenerator.Object, cryptoService.Object,
                configuration.Object);
        }

        [Test]
        public void Handle_SerialAlreadyExists_ThrowDuplicateException()
        {
            authValidationService.Setup(avs => avs.EmailExists(It.IsAny<string>())).ReturnsAsync(true);

            Assert.That(() => sendChangeEmailEmailByAdminCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<DuplicateException>());
        }

        [Test]
        public async Task Handle_WhenCalled_SendChangeEmailEmailByAdminResponse()
        {
            var result = await sendChangeEmailEmailByAdminCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendChangeEmailEmailByAdminResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}