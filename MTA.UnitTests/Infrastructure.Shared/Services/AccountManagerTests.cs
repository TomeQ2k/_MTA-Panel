using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using MTA.UnitTests.TestModels;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class AccountManagerTests
    {
        private User user;
        private Token token;
        private Mock<IDatabase> database;
        private Mock<IHashGenerator> hashGenerator;
        private Mock<IReadOnlyUserService> userService;
        private AccountManager accountManager;

        private const string Test = "test";

        [SetUp]
        public void SetUp()
        {
            user = new User();
            token = Token.Create(TokenType.ChangePassword, user.Id);
            user.SetToken(token);

            database = new Mock<IDatabase>();
            hashGenerator = new Mock<IHashGenerator>();
            userService = new Mock<IReadOnlyUserService>();
            var httpContextReader = new Mock<IHttpContextReader>();
            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), It.IsAny<TokenType>()))
                .ReturnsAsync(user);
            hashGenerator.Setup(h => h.CreateSalt(It.IsAny<int>())).Returns(It.IsNotNull<string>());
            hashGenerator.Setup(h => h.GenerateHash(It.IsAny<string>(), It.IsAny<string>())).Returns(Test);
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(true);
            database.Setup(d => d.TokenRepository.Delete(It.IsAny<Token>())).ReturnsAsync(true);
            database.Setup(d => d.SerialRepository.Insert(It.IsAny<Serial>(), true)).ReturnsAsync(true);
            database.Setup(d => d.TokenRepository.Delete(It.IsAny<Token>())).ReturnsAsync(true);

            accountManager = new AccountManager(database.Object, hashGenerator.Object, userService.Object,
                httpContextReader.Object);
        }

        #region ChangePassword

        [Test]
        public void ChangePassword_GettingTokenFailed_ThrowServerException()
        {
            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), It.IsAny<TokenType>()))
                .ReturnsAsync(() => null);

            Assert.That(() => accountManager.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void ChangePassword_NullTokenCode_ThrowServerException()
        {
            Assert.That(() => accountManager.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void ChangePassword_ExpiredToken_ThrowTokenExpiredException()
        {
            var testToken = new TestToken();
            testToken.ChangeCreationDate(-2);
            user.SetToken(testToken);
            Assert.That(() => accountManager.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), testToken.Code),
                Throws.TypeOf<TokenExpiredException>());
        }

        [Test]
        public void ChangePassword_UpdatingUserFailed_ThrowDatabase()
        {
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(false);

            Assert.That(() => accountManager.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), token.Code),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task ChangePassword_UpdatingUserPassedDeleteTokenFailed_ReturnFalse()
        {
            database.Setup(d => d.TokenRepository.Delete(It.IsAny<Token>())).ReturnsAsync(false);

            var result = await accountManager.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), token.Code);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ChangePassword_WhenCalled_ReturnTrue()
        {
            var result = await accountManager.ChangePassword(It.IsAny<string>(), It.IsAny<string>(), token.Code);

            Assert.That(result, Is.True);
        }

        #endregion

        #region ChangeEmail

        [Test]
        public void ChangeEmail_GettingTokenFailed_ThrowServerException()
        {
            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), It.IsAny<TokenType>()))
                .ReturnsAsync(() => null);

            Assert.That(() => accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void ChangeEmail_NullTokenCode_ThrowServerException()
        {
            Assert.That(() => accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void ChangeEmail_ExpiredToken_ThrowTokenExpiredException()
        {
            var testToken = new TestToken();
            testToken.ChangeCreationDate(-2);
            user.SetToken(testToken);
            Assert.That(() => accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), testToken.Code),
                Throws.TypeOf<TokenExpiredException>());
        }

        [Test]
        public void ChangeEmail_UpdatingUserFailed_ThrowDatabase()
        {
            database.Setup(d => d.UserRepository.Update(It.IsAny<User>())).ReturnsAsync(false);

            Assert.That(() => accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), token.Code),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task ChangeEmail_UpdatingUserPassedDeleteTokenFailed_ReturnFalse()
        {
            database.Setup(d => d.TokenRepository.Delete(It.IsAny<Token>())).ReturnsAsync(false);

            var result = await accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), token.Code);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ChangeEmail_WhenCalled_ReturnTrue()
        {
            var result = await accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), token.Code);

            Assert.That(result, Is.True);
        }

        #endregion

        #region AddSerial

        [Test]
        public void AddSerial_GettingTokenFailed_ThrowServerException()
        {
            database.Setup(d => d.UserRepository.GetUserByEmailWithTokenType(It.IsAny<string>(), It.IsAny<TokenType>()))
                .ReturnsAsync(() => null);

            Assert.That(() => accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void AddSerial_NullTokenCode_ThrowServerException()
        {
            Assert.That(() => accountManager.AddSerial(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public void AddSerial_ExpiredToken_ThrowTokenExpiredException()
        {
            var testToken = new TestToken();
            testToken.ChangeCreationDate(-2);
            user.SetToken(testToken);
            Assert.That(() => accountManager.AddSerial(It.IsAny<string>(), It.IsAny<string>(), testToken.Code),
                Throws.TypeOf<TokenExpiredException>());
        }

        [Test]
        public void AddSerial_InsertingSerialFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.SerialRepository.Insert(It.IsAny<Serial>(), true)).ReturnsAsync(false);
            Assert.That(() => accountManager.AddSerial(It.IsAny<string>(), It.IsAny<string>(), token.Code),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task AddSerial_InsertingSerialPassedDeletingToken_ReturnFalse()
        {
            database.Setup(d => d.TokenRepository.Delete(It.IsAny<Token>())).ReturnsAsync(false);

            var result = await accountManager.AddSerial(It.IsAny<string>(), It.IsAny<string>(), token.Code);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task AddSerial_WhenCalled_ReturnTrue()
        {
            var result = await accountManager.ChangeEmail(It.IsAny<string>(), It.IsAny<string>(), token.Code);

            Assert.That(result, Is.True);
        }

        #endregion
    }
}