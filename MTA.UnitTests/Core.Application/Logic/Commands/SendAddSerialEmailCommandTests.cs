using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class SendAddSerialEmailCommandTests
    {
        private SendAddSerialEmailRequest request;
        private Mock<ISerialService> serialService;
        private Mock<ICryptoService> cryptoService;
        private Mock<IEmailSender> emailSender;
        private Mock<IUserService> userService;
        private Mock<IUserTokenGenerator> userTokenGenerator;
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;

        private SendAddSerialEmailCommand sendAddSerialEmailCommand;
        private User user;

        private const string Test = "test";

        [SetUp]
        public void SetUp()
        {
            user = new User();
            user.Serials.Add(new Serial());
            user.Serials.Add(new Serial());
            user.IncreaseSerialsLimit(3);

            var generateAddSerialTokenResult = new GenerateAddSerialTokenResult
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

            request = new SendAddSerialEmailRequest
            {
                Serial = Test
            };

            serialService = new Mock<ISerialService>();
            cryptoService = new Mock<ICryptoService>();
            emailSender = new Mock<IEmailSender>();
            userService = new Mock<IUserService>();
            userTokenGenerator = new Mock<IUserTokenGenerator>();
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var configuration = new Mock<IConfiguration>();

            serialService.Setup(ss => ss.SerialExists(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(false);
            userTokenGenerator.Setup(c => c.GenerateAddSerialToken())
                .ReturnsAsync(generateAddSerialTokenResult);
            cryptoService.Setup(c => c.Encrypt(It.IsAny<string>()))
                .Returns(Test);
            configuration.Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);
            emailTemplateGenerator.Setup(etg => etg.FindEmailTemplate(It.IsAny<string>()))
                .ReturnsAsync(emailTemplate);
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(true);
            userService.Setup(us => us.GetUserWithSerials(It.IsAny<int>()))
                .ReturnsAsync(user);

            sendAddSerialEmailCommand = new SendAddSerialEmailCommand(serialService.Object, userService.Object,
                cryptoService.Object,
                emailSender.Object, userTokenGenerator.Object, emailTemplateGenerator.Object, httpContextReader.Object,
                configuration.Object);
        }

        [Test]
        public void Handle_UserHasNoMoreSlotsForSerial_ThrowPremiumOperationException()
        {
            user.IncreaseSerialsLimit(-1);
            
            Assert.That(() => sendAddSerialEmailCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<PremiumOperationException>());
        }

        [Test]
        public void Handle_SerialAlreadyExists_ThrowDuplicateException()
        {
            serialService.Setup(ss => ss.SerialExists(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            Assert.That(() => sendAddSerialEmailCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<DuplicateException>());
        }

        [Test]
        public async Task Handle_WhenCalled_SendAddSerialEmailResponse()
        {
            var result = await sendAddSerialEmailCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SendAddSerialEmailResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}