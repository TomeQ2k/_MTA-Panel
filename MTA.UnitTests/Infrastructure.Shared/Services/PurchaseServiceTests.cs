using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class PurchaseServiceTests
    {
        private PurchaseService purchaseService;

        private Mock<IDatabase> database;
        private Mock<IHttpContextReader> httpContextReader;

        [SetUp]
        public void SetUp()
        {
            database = new Mock<IDatabase>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.PurchaseRepository.Insert(It.IsNotNull<Purchase>(), true)).ReturnsAsync(true);

            purchaseService = new PurchaseService(database.Object, httpContextReader.Object);
        }

        #region CreatePurchase

        [Test]
        public void CreatePurchase_InsertingToDatabaseFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PurchaseRepository.Insert(It.IsNotNull<Purchase>(), true)).ReturnsAsync(false);

            Assert.That(() => purchaseService.CreatePurchase("name", 1), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreatePurchase_WhenCalled_ReturnPurchase()
        {
            var result = await purchaseService.CreatePurchase("name", 1);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<Purchase>());
        }

        #endregion
    }
}