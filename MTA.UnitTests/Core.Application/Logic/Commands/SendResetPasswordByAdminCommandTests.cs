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
    public class SendResetPasswordByAdminCommandTests
    {
        private SendResetPasswordByAdminCommand sendResetPasswordByAdminCommand;

        private Mock<IUserTokenGenerator> userTokenGenerator;
        private Mock<ICryptoService> cryptoService;
        private Mock<IEmailSender> emailSender;
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;
        private Mock<IConfiguration> configuration;

        private SendResetPasswordByAdminRequest request;

        private const string Test = "test";

        [SetUp]
        public void SetUp()
        {
            var generateResetPasswordTokenResult = new GenerateResetPasswordTokenResult
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

            request = new SendResetPasswordByAdminRequest {Login = Test};

            userTokenGenerator = new Mock<IUserTokenGenerator>();
            cryptoService = new Mock<ICryptoService>();
            emailSender = new Mock<IEmailSender>();
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();
            configuration = new Mock<IConfiguration>();

            userTokenGenerator.Setup(c => c.GenerateResetPasswordToken(It.IsAny<string>()))
                .ReturnsAsync(generateResetPasswordTokenResult);
            cryptoService.Setup(c => c.Encrypt(It.IsAny<string>()))
                .Returns(Test);
            configuration.Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);
            emailTemplateGenerator.Setup(etg => etg.FindEmailTemplate(It.IsAny<string>()))
                .ReturnsAsync(emailTemplate);
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(true);

            sendResetPasswordByAdminCommand = new SendResetPasswordByAdminCommand(userTokenGenerator.Object,
                cryptoService.Object, emailSender.Object, emailTemplateGenerator.Object, configuration.Object);
        }

        [Test]
        public void Handle_GenerateResetPasswordTokenFailed_ThrowCannotGenerateTokenException()
        {
            userTokenGenerator.Setup(c => c.GenerateResetPasswordToken(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => sendResetPasswordByAdminCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.Exception.TypeOf<CannotGenerateTokenException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnSendResetPasswordResponse()
        {
            var result = await sendResetPasswordByAdminCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendResetPasswordByAdminResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}