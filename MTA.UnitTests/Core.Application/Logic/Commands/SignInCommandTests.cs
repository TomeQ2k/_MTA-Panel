using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class SignInCommandTests
    {
        private Mock<IAuthService> authService;
        private Mock<IMapper> mapper;
        private SignInCommand signInCommand;

        [SetUp]
        public void SetUp()
        {
            authService = new Mock<IAuthService>();
            mapper = new Mock<IMapper>();

            signInCommand = new SignInCommand(authService.Object, mapper.Object);
        }

        [Test]
        public void Handle_SignInFailed_ThrowInvalidCredentialsException()
        {
            authService.Setup(a => a.SignIn(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => signInCommand.Handle(new SignInRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<InvalidCredentialsException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnSignInResponse()
        {
            var signInResponse = new SignInResult
            {
                JwtToken = "test",
                User = new User()
            };
            var mappedUser = new UserAuthDto();

            authService.Setup(a => a.SignIn(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(signInResponse);
            mapper.Setup(m => m.Map<UserAuthDto>(It.IsAny<User>()))
                .Returns(mappedUser);

            var result = await signInCommand.Handle(new SignInRequest(), It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<SignInResponse>());
            Assert.That(result.Token, Is.EqualTo(signInResponse.JwtToken));
            Assert.That(result.User, Is.EqualTo(mappedUser));
        }
    }
}