using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class OrderServiceTests
    {
        private OrderService orderService;

        private Mock<IDatabase> database;
        private Mock<IRolesService> rolesService;
        private Mock<IHttpContextReader> httpContextReader;

        private Order order;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            order = new Order();
            order.SetUser(new User());

            database = new Mock<IDatabase>();
            rolesService = new Mock<IRolesService>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.OrderRepository.Insert(order, false)).ReturnsAsync(true);
            database.Setup(d => d.OrderRepository.FindById(It.IsNotNull<string>())).ReturnsAsync(order);
            database.Setup(d => d.OrderRepository.Update(order)).ReturnsAsync(true);
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllAdminsRoles)).ReturnsAsync(true);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            orderService = new OrderService(database.Object, rolesService.Object, httpContextReader.Object);
        }

        #region CreateOrder

        [Test]
        public void CreateOrder_OrderIsNull_ThrowServerException()
        {
            order = null;

            Assert.That(() => orderService.CreateOrder(order), Throws.Exception.TypeOf<ServerException>());
        }

        [Test]
        public void CreateOrder_InsertingToDatabaseFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.OrderRepository.Insert(order, false)).ReturnsAsync(false);

            Assert.That(() => orderService.CreateOrder(order), Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateOrder_WhenCalled_ReturnOrder()
        {
            var result = await orderService.CreateOrder(order);

            Assert.That(result, Is.Not.Null
                .And.TypeOf<Order>());
        }

        #endregion

        #region SetOrderApprovalState

        [Test]
        public void SetOrderApprovalState_UserIsNotPermitted_ThrowNoPermissionsException()
        {
            rolesService.Setup(r => r.IsPermitted(UserId, Constants.AllAdminsRoles)).ReturnsAsync(false);

            Assert.That(() => orderService.SetOrderApprovalState(order.Id, It.IsAny<StateType>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void SetOrderApprovalState_OrderNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.OrderRepository.FindById(It.IsNotNull<string>())).ReturnsAsync(() => null);

            Assert.That(() => orderService.SetOrderApprovalState(order.Id, StateType.Accept),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void SetOrderApprovalState_OrderIsAlreadyReviewed_ThrowNoPermissionsException()
        {
            order.SetApprovalState(StateType.Accept);

            Assert.That(() => orderService.SetOrderApprovalState(order.Id, StateType.Accept),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public async Task SetOrderApprovalState_WhenCalled_ReturnIsOrderUpdated()
        {
            var result = await orderService.SetOrderApprovalState(order.Id, StateType.Accept);

            Assert.That(result, Is.TypeOf<SetApprovalStateResult>());
        }

        #endregion
    }
}