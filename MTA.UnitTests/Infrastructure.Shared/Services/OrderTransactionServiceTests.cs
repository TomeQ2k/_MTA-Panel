using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Models;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class OrderTransactionServiceTests
    {
        private Mock<IDatabase> database;
        private OrderTransactionService orderTransactionService;

        [SetUp]
        public void SetUp()
        {
            database = new Mock<IDatabase>();

            database.Setup(d => d.OrderTransactionRepository.Insert(It.IsAny<OrderTransaction>(), true))
                .ReturnsAsync(true);

            orderTransactionService = new OrderTransactionService(database.Object);
        }

        #region CreateOrderTransaction

        [Test]
        public void CreateOrderTransaction_InsertingOrderTransactionFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.OrderTransactionRepository.Insert(It.IsAny<OrderTransaction>(), true))
                .ReturnsAsync(false);

            Assert.That(() => orderTransactionService.CreateOrderTransaction("test", 10,
                new EmailUsernameTuple("test", "test")), Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateOrderTransaction_WhenCalled_ReturnOrderTransaction()
        {
            var result =
                await orderTransactionService.CreateOrderTransaction("test", 10,
                    new EmailUsernameTuple("test", "test"));

            Assert.That(result, Is.TypeOf<OrderTransaction>());
        }

        #endregion
    }
}