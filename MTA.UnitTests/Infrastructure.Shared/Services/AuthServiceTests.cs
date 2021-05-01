using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Database;
using MTA.Infrastructure.Shared.Services;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private User user;
        private Mock<IDatabase> database;
        private Mock<IJwtAuthorizationTokenGenerator> jwtAuthorizationTokenGenerator;
        private Mock<IHashGenerator> hashGenerator;
        private AuthService authService;

        [SetUp]
        public void SetUp()
        {
            user = new User();
            database = new Mock<IDatabase>();
            jwtAuthorizationTokenGenerator = new Mock<IJwtAuthorizationTokenGenerator>();
            hashGenerator = new Mock<IHashGenerator>();

            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());

            authService = new AuthService(database.Object, jwtAuthorizationTokenGenerator.Object, hashGenerator.Object);
        }

        #region SignIn

        [Test]
        public void SignIn_AccountWithThatEmailDoesntExists_ThrowInvalidCredentialsException()
        {
            database.Setup(d => d.UserRepository.FindUserByEmail(It.IsAny<string>()))
                .Throws(new InvalidCredentialsException(It.IsAny<string>()));

            Assert.That(() => authService.SignIn("test@test.pl", It.IsAny<string>()),
                Throws.TypeOf<InvalidCredentialsException>());
        }

        [Test]
        public void SignIn_AccountWithThatUsernameDoesntExists_ThrowInvalidCredentialsException()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsAny<string>()))
                .Throws(new InvalidCredentialsException(It.IsAny<string>()));

            Assert.That(() => authService.SignIn("test", It.IsAny<string>()),
                Throws.TypeOf<InvalidCredentialsException>());
        }

        [Test]
        public void SignIn_AccountIsAlreadyActivated_ThrowAccountNotConfirmedException()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername("test"))
                .ReturnsAsync(user);

            Assert.That(() => authService.SignIn("test", It.IsAny<string>()),
                Throws.TypeOf<AccountNotConfirmedException>());
        }

        [Test]
        public void SignIn_IncorrectPassword_ThrowInvalidCredentialsException()
        {
            user.ActivateAccount();
            user.SetBanId(null);

            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(user);
            hashGenerator.Setup(hg => hg.VerifyHash(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            Assert.That(() => authService.SignIn("test", It.IsAny<string>()),
                Throws.TypeOf<InvalidCredentialsException>());
        }

        [Test]
        public async Task SignIn_WhenCalled_ReturnSignInResult()
        {
            user.ActivateAccount();
            user.SetBanId(null);

            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsAny<string>()))
                .ReturnsAsync(user);
            hashGenerator.Setup(hg => hg.VerifyHash(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            jwtAuthorizationTokenGenerator.Setup(jatg => jatg.GenerateToken(user)).Returns("testToken");

            var result = await authService.SignIn("test", It.IsAny<string>());
            Assert.That(result, Is.EqualTo(new SignInResult {JwtToken = "testToken", User = user}));
        }

        #endregion

        #region SignUp

        [Test]
        public void SignUp_DatabaseUserInsertFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.UserRepository.Insert(It.IsAny<User>(), true)).ReturnsAsync(false);

            Assert.That(() => authService.SignUp(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void SignUp_DatabaseUserInsertPassedSerialInsertFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.UserRepository.Insert(It.IsAny<User>(), true)).ReturnsAsync(true);
            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsAny<string>())).ReturnsAsync(user);
            database.Setup(d => d.SerialRepository.Insert(It.IsAny<Serial>(), true)).ReturnsAsync(false);

            Assert.That(() => authService.SignUp(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void SignUp_DatabaseUserInsertPassedSerialInsertPassedTokenInsertFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.UserRepository.Insert(It.IsAny<User>(), true)).ReturnsAsync(true);
            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsAny<string>())).ReturnsAsync(user);
            database.Setup(d => d.SerialRepository.Insert(It.IsAny<Serial>(), true)).ReturnsAsync(true);
            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false)).ReturnsAsync(false);

            Assert.That(() => authService.SignUp(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task SignUp_DatabaseUserInsertPassedSerialInsertTokenInsertPassed_ReturnSignUpResult()
        {
            database.Setup(d => d.UserRepository.Insert(It.IsAny<User>(), true)).ReturnsAsync(true);
            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsAny<string>())).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false)).ReturnsAsync(true);
            database.Setup(d => d.SerialRepository.Insert(It.IsAny<Serial>(), true)).ReturnsAsync(true);

            var result = await authService.SignUp(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>());

            Assert.That(result.User, Is.EqualTo(user));
        }

        #endregion

        #region ConfirmAccount

        [Test]
        public void ConfirmAccount_UserNotFound_ThrowServerException()
        {
            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), TokenType.Register))
                .ReturnsAsync(It.IsAny<User>());

            Assert.That(() => authService.ConfirmAccount(It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void ConfirmAccount_TokensNotMatch_ThrowServerException()
        {
            var token = Token.Create(TokenType.Register, user.Id);
            user.SetToken(token);

            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), TokenType.Register))
                .ReturnsAsync(user);

            Assert.That(() => authService.ConfirmAccount(It.IsAny<string>(), It.IsNotNull<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void ConfirmAccount_UserTokenHasExpired_ThrowTokenExpiredException()
        {
            var token = new TestToken();
            token.ChangeCreationDate(-2);
            user.SetToken(token);

            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), TokenType.Register))
                .ReturnsAsync(user);

            Assert.That(() => authService.ConfirmAccount(It.IsAny<string>(), token.Code),
                Throws.TypeOf<TokenExpiredException>());
        }

        #endregion

        #region GenerateActivationAccountToken

        [Test]
        public async Task GenerateActivationAccountToken_UserNotFound_ReturnNull()
        {
            database.Setup(d => d.UserRepository.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(It.IsAny<User>());

            var result = await authService.GenerateActivationEmailToken(It.IsAny<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GenerateActivationAccountToken_ActivatedAccount_ReturnNull()
        {
            database.Setup(d => d.UserRepository.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(user);

            var result = await authService.GenerateActivationEmailToken(It.IsAny<string>());

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GenerateActivationAccountToken_ValidScenerio_ReturnSendActivationEmailResult()
        {
            user.ActivateAccount();
            user.SetEmail("test");
            user.SetUsername("test");

            database.Setup(d => d.UserRepository.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.GetTokenWithTypeByUserId(It.IsAny<int>(), TokenType.Register))
                .ReturnsAsync(It.IsAny<Token>());

            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false)).ReturnsAsync(true);

            var result = await authService.GenerateActivationEmailToken(It.IsAny<string>());

            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        [Test]
        public async Task GenerateActivationAccountToken_InsertTokenFailed_ReturnNull()
        {
            user.SetEmail("test");
            user.SetUsername("test");
            database.Setup(d => d.UserRepository.FindUserByEmail(It.IsAny<string>())).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.GetTokenWithTypeByUserId(It.IsAny<int>(), TokenType.Register))
                .ReturnsAsync(It.IsAny<Token>());

            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false)).ReturnsAsync(false);

            var result = await authService.GenerateActivationEmailToken(It.IsAny<string>());

            Assert.That(result, Is.Null);
        }

        #endregion
    }
}