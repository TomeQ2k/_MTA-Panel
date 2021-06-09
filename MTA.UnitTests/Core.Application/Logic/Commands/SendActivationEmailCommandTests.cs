using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class SendActivationEmailCommandTests
    {
        private Mock<IAuthService> authService;
        private Mock<ICryptoService> cryptoService;
        private Mock<IEmailSender> emailSender;
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;
        private Mock<IConfiguration> configuration;
        private SendActivationEmailCommand sendActivationEmailCommand;
        private SendActivationEmailRequest request;

        private const string Test = "test";

        [SetUp]
        public void SetUp()
        {
            var sendActivationEmailResult = new SendActivationEmailResult
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

            request = new SendActivationEmailRequest
            {
                Email = Test
            };

            authService = new Mock<IAuthService>();
            cryptoService = new Mock<ICryptoService>();
            emailSender = new Mock<IEmailSender>();
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();
            configuration = new Mock<IConfiguration>();

            authService.Setup(a => a.GenerateActivationEmailToken(It.IsNotNull<string>()))
                .ReturnsAsync(sendActivationEmailResult);
            cryptoService.Setup(c => c.Encrypt(It.IsAny<string>()))
                .Returns(Test);
            configuration.Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);
            emailTemplateGenerator.Setup(etg => etg.FindEmailTemplate(It.IsAny<string>()))
                .ReturnsAsync(emailTemplate);
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(true);

            sendActivationEmailCommand = new SendActivationEmailCommand(authService.Object, cryptoService.Object,
                emailSender.Object, emailTemplateGenerator.Object, configuration.Object);
        }

        [Test]
        public async Task Handle_GenerateActivatioEmailTokenFailed_ReturnEmptySendActivationEmailResponse()
        {
            authService.Setup(a => a.GenerateActivationEmailToken(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            var result = await sendActivationEmailCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendActivationEmailResponse>());
            Assert.That(result.Email, Is.Null);
            Assert.That(result.Token, Is.Null);
        }

        [Test]
        public async Task Handle_SendingEmailFailed_ReturnEmptySendActivationEmailResponse()
        {
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(false);

            var result = await sendActivationEmailCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendActivationEmailResponse>());
            Assert.That(result.Email, Is.Null);
            Assert.That(result.Token, Is.Null);
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnSendActivationEmailResponse()
        {
            var result = await sendActivationEmailCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendActivationEmailResponse>());
            Assert.That(result.Email, Is.Not.Null);
            Assert.That(result.Token, Is.Not.Null);
        }
    }
}