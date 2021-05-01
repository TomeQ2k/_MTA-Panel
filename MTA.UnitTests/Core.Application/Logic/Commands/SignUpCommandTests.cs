using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Moq;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class SignUpCommandTests
    {
        private SignUpCommand signUpCommand;

        private Mock<IAuthService> authService;
        private Mock<ISerialService> serialService;
        private Mock<IReadOnlyUserService> userService;
        private Mock<IEmailSender> emailSender;
        private Mock<ICryptoService> cryptoService;
        private Mock<IEmailTemplateGenerator> emailTemplateGenerator;
        private Mock<IAuthValidationService> authValidationService;
        private Mock<ICaptchaService> captchaService;
        private Mock<IMapper> mapper;
        private Mock<IConfiguration> configuration;

        private SignUpRequest request;

        private const string Test = "test";

        [SetUp]
        public void SetUp()
        {
            var user = new User();
            var result = new SignUpResult
            {
                TokenCode = Test,
                User = user
            };
            var emailTemplate = new EmailTemplate
            {
                TemplateName = Test,
                Subject = Test,
                Content = Test,
                AllowedParameters = new[] {"{{username}}", "{{callbackUrl}}"}
            };

            request = new SignUpRequest();

            authService = new Mock<IAuthService>();
            serialService = new Mock<ISerialService>();
            userService = new Mock<IReadOnlyUserService>();
            emailSender = new Mock<IEmailSender>();
            cryptoService = new Mock<ICryptoService>();
            emailTemplateGenerator = new Mock<IEmailTemplateGenerator>();
            authValidationService = new Mock<IAuthValidationService>();
            captchaService = new Mock<ICaptchaService>();
            mapper = new Mock<IMapper>();
            configuration = new Mock<IConfiguration>();

            authService.Setup(a => a.SignUp(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(result);
            captchaService.Setup(c => c.VerifyCaptcha(It.IsAny<string>())).ReturnsAsync(true);
            authValidationService.Setup(a => a.UsernameExists(It.IsAny<string>())).ReturnsAsync(false);
            authValidationService.Setup(a => a.EmailExists(It.IsAny<string>())).ReturnsAsync(false);
            serialService.Setup(s => s.SerialExists(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(false);
            userService.Setup(u => u.FindUserByUsername(It.IsAny<string>())).ReturnsAsync(user);
            cryptoService.Setup(c => c.Encrypt(It.IsAny<string>())).Returns(Test);
            emailTemplateGenerator.Setup(etg => etg.FindEmailTemplate(It.IsAny<string>()))
                .ReturnsAsync(emailTemplate);
            emailSender.Setup(es => es.Send(It.IsAny<EmailMessage>()))
                .ReturnsAsync(true);
            configuration.Setup(c => c.GetSection(It.IsAny<string>()))
                .Returns(new Mock<IConfigurationSection>().Object);

            signUpCommand = new SignUpCommand(authService.Object, serialService.Object, userService.Object,
                emailSender.Object, cryptoService.Object, emailTemplateGenerator.Object, authValidationService.Object,
                captchaService.Object, configuration.Object, mapper.Object);
        }

        [Test]
        public void Handle_VerifyCaptchaFailed_ThrowCaptchaException()
        {
            captchaService.Setup(c => c.VerifyCaptcha(It.IsAny<string>())).ReturnsAsync(false);

            Assert.That(() => signUpCommand.Handle(new SignUpRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<CaptchaException>());
        }

        [Test]
        public void Handle_UsernameAlreadyExists_ThrowDuplicateException()
        {
            authValidationService.Setup(a => a.UsernameExists(It.IsAny<string>())).ReturnsAsync(true);

            Assert.That(() => signUpCommand.Handle(new SignUpRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<DuplicateException>());
        }

        [Test]
        public void Handle_EmailAlreadyExists_ThrowDuplicateException()
        {
            authValidationService.Setup(a => a.EmailExists(It.IsAny<string>())).ReturnsAsync(true);

            Assert.That(() => signUpCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<DuplicateException>());
        }

        [Test]
        public async Task Handle_ReferrerGiven_FindReferrerShouldBeCalled()
        {
            await signUpCommand.Handle(request with {Referrer = Test}, It.IsAny<CancellationToken>());

            userService.Verify(u => u.FindUserByUsername(Test));
        }

        [Test]
        public void Handle_SendEmailFailed_ThrowServiceException()
        {
            emailSender.Setup(e => e.Send(It.IsAny<EmailMessage>())).ReturnsAsync(false);

            Assert.That(() => signUpCommand.Handle(request, It.IsAny<CancellationToken>()),
                Throws.TypeOf<ServiceException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnSignUpResponse()
        {
            var mappedUser = new UserAuthDto();
            mapper.Setup(m => m.Map<UserAuthDto>(It.IsAny<User>())).Returns(mappedUser);

            var result = await signUpCommand.Handle(request, It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SignUpResponse>());
            Assert.That(result.TokenCode, Is.Not.Null.And.TypeOf<string>());
            Assert.That(result.User, Is.Not.Null.And.TypeOf<UserAuthDto>());
        }
    }
}