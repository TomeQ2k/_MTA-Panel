using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class UserTokenGeneratorTests
    {
        private UserTokenGenerator userTokenGenerator;

        private Mock<IDatabase> database;
        private Mock<IReadOnlyAccountManager> accountManager;
        private Mock<IHashGenerator> hashGenerator;

        private User user;

        [SetUp]
        public void SetUp()
        {
            user = new UserBuilder()
                .SetUsername("test")
                .SetEmail("test")
                .Build();

            database = new Mock<IDatabase>();
            accountManager = new Mock<IReadOnlyAccountManager>();
            hashGenerator = new Mock<IHashGenerator>();

            database.Setup(d => d.UserRepository.FindUserByUsername(It.IsNotNull<string>())).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.Insert(It.IsNotNull<Token>(), false)).ReturnsAsync(true);
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(user);
            accountManager.Setup(am => am.GetCurrentUser()).ReturnsAsync(user);
            hashGenerator
                .Setup(hg => hg.VerifyHash(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);

            userTokenGenerator = new UserTokenGenerator(database.Object, accountManager.Object, hashGenerator.Object);
        }

        #region GenerateResetPasswordToken

        [Test]
        public void GenerateResetPasswordToken_FindUserByEmail_ShouldBeCalled()
        {
            database.Setup(d => d.UserRepository.FindUserByEmail("test@test.pl")).ReturnsAsync(new User());
            userTokenGenerator.GenerateResetPasswordToken("test@test.pl");
            database.Verify(d => d.UserRepository.FindUserByEmail("test@test.pl"));
        }

        [Test]
        public void GenerateResetPasswordToken_FindUserByUsername_ShouldBeCalled()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(new User());
            userTokenGenerator.GenerateResetPasswordToken("test");
            database.Verify(d => d.UserRepository.FindUserByUsername("test"));
        }

        [Test]
        public void GenerateResetPasswordToken_GetTokenWithTypeByUserId_ShouldBeCalled()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword))
                .ReturnsAsync(new Token());
            userTokenGenerator.GenerateResetPasswordToken("test");
            database.Verify(d => d.UserRepository.FindUserByUsername("test"));
            database.Verify(d => d.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword));
        }

        [Test]
        public void GenerateResetPasswordToken_DeleteToken_ShouldBeCalled()
        {
            var token = new Token();

            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword))
                .ReturnsAsync(token);
            database.Setup(d => d.TokenRepository.Delete(user.Token));

            userTokenGenerator.GenerateResetPasswordToken("test");

            database.Verify(d => d.UserRepository.FindUserByUsername("test"));
            database.Verify(d => d.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword));
            database.Verify(d => d.TokenRepository.Delete(It.IsAny<Token>()));
        }

        [Test]
        public async Task GenerateResetPasswordToken_EmptyUser_ReturnNull()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(It.IsAny<User>());

            var result = await userTokenGenerator.GenerateResetPasswordToken("test");

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GenerateResetPasswordToken_AddsTokenProperly_ReturnGenerateResetPasswordTokenResult()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword))
                .ReturnsAsync(It.IsAny<Token>());
            database.Setup(d => d.TokenRepository.Insert(It.IsNotNull<Token>(), false)).ReturnsAsync(true);

            var result = await userTokenGenerator.GenerateResetPasswordToken("test");

            Assert.That(result, Is.TypeOf<GenerateResetPasswordTokenResult>());
        }

        [Test]
        public void GenerateResetPasswordToken_DatabaseError_ThrowDatabaseException()
        {
            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(user);
            database.Setup(d => d.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword))
                .ReturnsAsync(It.IsAny<Token>());
            database.Setup(d => d.TokenRepository.Insert(It.IsNotNull<Token>(), false)).ReturnsAsync(false);

            Assert.That(() => userTokenGenerator.GenerateResetPasswordToken("test"),
                Throws.TypeOf<DatabaseException>());
        }

        #endregion

        #region GenerateChangePasswordToken

        [Test]
        public void GenerateChangePasswordToken_GetEmptyUser_ThrowEntityNotFoundException()
        {
            accountManager.Setup(am => am.GetCurrentUser()).ReturnsAsync(() => null);

            Assert.That(() => userTokenGenerator.GenerateChangePasswordToken(It.IsAny<string>()),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void GenerateChangePasswordToken_VerifingHashFailed_ThrowOldPasswordInvalidException()
        {
            hashGenerator.Setup(hs => hs.VerifyHash(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            Assert.That(() => userTokenGenerator.GenerateChangePasswordToken(It.IsAny<string>()),
                Throws.TypeOf<OldPasswordInvalidException>());
        }

        [Test]
        public void GenerateChangePasswordToken_InsertingTokenFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false))
                .ReturnsAsync(false);

            Assert.That(() => userTokenGenerator.GenerateChangePasswordToken(It.IsAny<string>()),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task GenerateChangePasswordToken_WhenCalled_ReturnGenerateChangePasswordTokenResult()
        {
            var result = await userTokenGenerator.GenerateChangePasswordToken(It.IsAny<string>());

            Assert.That(result, Is.TypeOf<GenerateChangePasswordTokenResult>().And.Not.Null);
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        #endregion

        #region GenerateChangeEmailToken

        [Test]
        public void GenerateChangeEmailToken_GetEmptyUser_ThrowEntityNotFoundException()
        {
            accountManager.Setup(am => am.GetCurrentUser()).ReturnsAsync(() => null);

            Assert.That(() => userTokenGenerator.GenerateChangeEmailToken(),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void GenerateChangeEmailToken_InsertingTokenFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false))
                .ReturnsAsync(false);

            Assert.That(() => userTokenGenerator.GenerateChangeEmailToken(),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task GenerateChangeEmailToken_WhenCalled_ReturnGenerateChangePasswordTokenResult()
        {
            var result = await userTokenGenerator.GenerateChangeEmailToken();

            Assert.That(result, Is.TypeOf<GenerateChangeEmailTokenResult>().And.Not.Null);
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        #endregion

        #region GenerateChangeEmailTokenByAdmin

        [Test]
        public void GenerateChangeEmailByAdminToken_GetEmptyUser_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => userTokenGenerator.GenerateChangeEmailTokenByAdmin(It.IsAny<int>(), It.IsAny<string>()),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void GenerateChangeEmailByAdminToken_InsertingTokenFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false))
                .ReturnsAsync(false);

            Assert.That(() => userTokenGenerator.GenerateChangeEmailTokenByAdmin(It.IsAny<int>(), It.IsAny<string>()),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task GenerateChangeEmailByAdminToken_WhenCalled_ReturnGenerateChangePasswordTokenResult()
        {
            var email = "email";
            var result = await userTokenGenerator.GenerateChangeEmailTokenByAdmin(It.IsAny<int>(), email);

            Assert.That(result, Is.TypeOf<GenerateChangeEmailTokenResult>().And.Not.Null);
            Assert.That(result.Email, Is.EqualTo(email));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        #endregion

        #region GenerateAddSerialToken

        [Test]
        public void GenerateAddSerialToken_GetEmptyUser_ThrowEntityNotFoundException()
        {
            accountManager.Setup(am => am.GetCurrentUser()).ReturnsAsync(() => null);

            Assert.That(() => userTokenGenerator.GenerateAddSerialToken(),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void GenerateAddSerialToken_InsertingTokenFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.TokenRepository.Insert(It.IsAny<Token>(), false))
                .ReturnsAsync(false);

            Assert.That(() => userTokenGenerator.GenerateAddSerialToken(),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task GenerateAddSerialToken_WhenCalled_ReturnGenerateChangePasswordTokenResult()
        {
            var result = await userTokenGenerator.GenerateAddSerialToken();

            Assert.That(result, Is.TypeOf<GenerateAddSerialTokenResult>().And.Not.Null);
            Assert.That(result.Email, Is.EqualTo(user.Email));
            Assert.That(result.Username, Is.EqualTo(user.Username));
        }

        #endregion
    }
}