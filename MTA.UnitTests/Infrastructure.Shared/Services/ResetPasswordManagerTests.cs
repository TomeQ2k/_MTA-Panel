using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ResetPasswordManagerTests
    {
        private User user;
        private Mock<IDatabase> database;
        private Mock<IHashGenerator> hashGenerator;
        private ResetPasswordManager service;

        [SetUp]
        public void SetUp()
        {
            user = new User();

            database = new Mock<IDatabase>();
            database.Setup(d => d.UserRepository.FindUserByUsername("test")).ReturnsAsync(user);
            hashGenerator = new Mock<IHashGenerator>();
            service = new ResetPasswordManager(database.Object, hashGenerator.Object);
        }

        #region ResetPassword

        [Test]
        public async Task ResetPassword_WhenCalledValid_ReturnTrue()
        {
            var token = Token.Create(TokenType.ResetPassword, user.Id);
            user.SetToken(token);
            database.Setup(d =>
                    d.UserRepository.GetUserByEmailWithTokenType("test", It.IsNotNull<TokenType>()))
                .ReturnsAsync(user);
            database.Setup(d => d.UserRepository.Update(It.IsNotNull<User>())).ReturnsAsync(true);
            database.Setup(d => d.TokenRepository.Delete(It.IsNotNull<Token>())).ReturnsAsync(true);


            var result =
                await service.ResetPassword("test", user.Token.Code, It.IsNotNull<string>());

            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void ResetPassword_DatabaseError_ThrowDatabaseException()
        {
            var token = Token.Create(TokenType.ResetPassword, user.Id);
            user.SetToken(token);
            database.Setup(d =>
                    d.UserRepository.GetUserByEmailWithTokenType("test", It.IsNotNull<TokenType>()))
                .ReturnsAsync(user);
            database.Setup(d => d.UserRepository.Update(It.IsNotNull<User>())).ReturnsAsync(false);

            Assert.That(() => service.ResetPassword("test", user.Token.Code, It.IsNotNull<string>()),
                Throws.TypeOf<DatabaseException>());
        }

        #endregion
    }
}