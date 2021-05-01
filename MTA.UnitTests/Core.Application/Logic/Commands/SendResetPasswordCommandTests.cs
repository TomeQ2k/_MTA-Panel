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
    public class SendResetPasswordCommandTests
    {
        private Mock<IUserTokenGenerator> userTokenGenerator;
        private Mock<ICryptoService> cryptoService;
        private Mock<IEmailSender> emailSender;
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;
        private Mock<IConfiguration> configuration;
        private Mock<ICaptchaService> captchaService;
        private SendResetPasswordCommand sendResetPasswordCommand;
        private SendResetPasswordRequest request;

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

            request = new SendResetPasswordRequest
            {
                Login = Test,
                CaptchaResponse = Test
            };

            userTokenGenerator = new Mock<IUserTokenGenerator>();
            cryptoService = new Mock<ICryptoService>();
            emailSender = new Mock<IEmailSender>();
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();
            captchaService = new Mock<ICaptchaService>();
            configuration = new Mock<IConfiguration>();


            userTokenGenerator.Setup(c => c.GenerateResetPasswordToken(It.IsAny<string>()))
                .ReturnsAsync(generateResetPasswordTokenResult);
            captchaService.Setup(c => c.VerifyCaptcha(It.IsAny<string>()))
                .ReturnsAsync(true);
            cryptoService.Setup(c => c.Encrypt(It.IsAny<string>()))
                .Returns(Test);
            configuration.Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);
            emailTemplateGenerator.Setup(etg => etg.FindEmailTemplate(It.IsAny<string>()))
                .ReturnsAsync(emailTemplate);
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(true);

            sendResetPasswordCommand = new SendResetPasswordCommand(userTokenGenerator.Object, cryptoService.Object,
                emailSender.Object, emailTemplateGenerator.Object, captchaService.Object, configuration.Object);
        }

        [Test]
        public void Handle_VerifyCaptchaFailed_ThrowCaptchaException()
        {
            captchaService.Setup(c => c.VerifyCaptcha(It.IsAny<string>()))
                .ReturnsAsync(false);

            Assert.That(() => sendResetPasswordCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<CaptchaException>());
        }

        [Test]
        public async Task Handle_GenerateResetPasswordTokenFailed_ReturnSendResetPasswordResponse()
        {
            userTokenGenerator.Setup(c => c.GenerateResetPasswordToken(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var result = await sendResetPasswordCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendResetPasswordResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnSendResetPasswordResponse()
        {
            var result = await sendResetPasswordCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendResetPasswordResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}