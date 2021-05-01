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
    public class DonationManagerTests
    {
        private DonationManager donationManager;

        private Mock<IDatabase> database;
        private Mock<IUserManager> userManager;
        private Mock<IHttpContextReader> httpContextReader;

        private TestToken token;
        private OrderTransaction orderTransaction;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            token = new TestToken();
            orderTransaction = new OrderTransaction();
            var addCreditsResult = new AddCreditsResult(It.IsAny<int>());

            database = new Mock<IDatabase>();
            userManager = new Mock<IUserManager>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d =>
                    d.TokenRepository.GetTokenByCodeAndType(It.IsNotNull<string>(), TokenType.Payment, UserId))
                .ReturnsAsync(token);
            database.Setup(d => d.OrderTransactionRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(orderTransaction);
            database.Setup(d => d.TokenRepository.Delete(It.IsNotNull<Token>())).ReturnsAsync(true);
            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());
            database.Setup(d => d.ReportRepository.Insert(It.IsNotNull<Report>(), false)).ReturnsAsync(true);
            userManager.Setup(u => u.AddCredits(It.IsNotNull<int>(), UserId)).ReturnsAsync(addCreditsResult);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            donationManager = new DonationManager(database.Object, userManager.Object, httpContextReader.Object);
        }

        #region DonateServer

        [Test]
        public void DonateServer_TokenNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d =>
                    d.TokenRepository.GetTokenByCodeAndType(It.IsNotNull<string>(), TokenType.Payment, UserId))
                .ReturnsAsync(() => null);

            Assert.That(() => donationManager.DonateServer(DonationType.FiftyPLN, "code"),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void DonateServer_TokenHasExpired_ThrowTokenExpiredException()
        {
            database.Setup(d =>
                    d.TokenRepository.GetTokenByCodeAndType(It.IsNotNull<string>(), TokenType.Payment, UserId))
                .ReturnsAsync(() => { return token.ChangeCreationDate(-3); });

            Assert.That(() => donationManager.DonateServer(DonationType.FiftyPLN, "code"),
                Throws.Exception.TypeOf<TokenExpiredException>());
        }

        [Test]
        public void DonateServer_AddingCreditsFailed_ThrowDatabaseException()
        {
            userManager.Setup(u => u.AddCredits(It.IsNotNull<int>(), UserId)).ReturnsAsync(() => null);

            Assert.That(() => donationManager.DonateServer(DonationType.FiftyPLN, "code"),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void DonateServer_OrderTransactionNotExists_ThrowServerException()
        {
            database.Setup(d => d.OrderTransactionRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => donationManager.DonateServer(DonationType.FiftyPLN, "code"),
                Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void DonateServer_DeletingTokenFailed_ThrowDatabaseException()
        {
            orderTransaction.Validate();

            database.Setup(d => d.TokenRepository.Delete(It.IsNotNull<Token>())).ReturnsAsync(false);

            Assert.That(() => donationManager.DonateServer(DonationType.FiftyPLN, "code"),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task DonateServer_WhenCalled_ReturnDonateServerResult()
        {
            orderTransaction.Validate();

            var result = await donationManager.DonateServer(DonationType.FiftyPLN, "code");

            Assert.That(result, Is.Not.Null
                .And.TypeOf<DonateServerResult>());
            Assert.That(result.IsSucceeded, Is.True);
        }

        #endregion

        #region DonateServerDlcBrain

        [Test]
        public void DonateServerDlcBrain_TokenNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d =>
                    d.TokenRepository.GetTokenByCodeAndType(It.IsNotNull<string>(), TokenType.Payment, UserId))
                .ReturnsAsync(() => null);

            Assert.That(() => donationManager.DonateServerDlcBrain("code"),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void DonateServerDlcBrain_TokenHasExpired_ThrowTokenExpiredException()
        {
            database.Setup(d =>
                    d.TokenRepository.GetTokenByCodeAndType(It.IsNotNull<string>(), TokenType.Payment, UserId))
                .ReturnsAsync(() => { return token.ChangeCreationDate(-3); });

            Assert.That(() => donationManager.DonateServerDlcBrain("code"),
                Throws.Exception.TypeOf<TokenExpiredException>());
        }

        [Test]
        public void DonateServerDlcBrain_InsertingDonationReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportRepository.Insert(It.IsNotNull<Report>(), false)).ReturnsAsync(false);

            Assert.That(() => donationManager.DonateServerDlcBrain("code"),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public void DonateServerDlcBrain_OrderTransactionNotExists_ThrowServerException()
        {
            database.Setup(d => d.OrderTransactionRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => donationManager.DonateServerDlcBrain("code"),
                Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void DonateServerDlcBrain_DeletingTokenFailed_ThrowDatabaseException()
        {
            orderTransaction.Validate();

            database.Setup(d => d.TokenRepository.Delete(It.IsNotNull<Token>())).ReturnsAsync(false);

            Assert.That(() => donationManager.DonateServerDlcBrain("code"),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task DonateServerDlcBrain_WhenCalled_ReturnDonateServerResult()
        {
            orderTransaction.Validate();

            var result = await donationManager.DonateServerDlcBrain("code");

            Assert.That(result, Is.Not.Null
                .And.TypeOf<DonateServerResult>());
            Assert.That(result.IsSucceeded, Is.True);
        }

        #endregion
    }
}