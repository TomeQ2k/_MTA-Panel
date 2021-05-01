using System.Data;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class OrderService : IOrderService
    {
        private readonly IDatabase database;
        private readonly IRolesService rolesService;
        private readonly IHttpContextReader httpContextReader;

        public OrderService(IDatabase database, IRolesService rolesService, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.rolesService = rolesService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<PagedList<Order>> GetOrders(OrderFiltersParams filtersParams)
            => (await database.OrderRepository.GetOrders(filtersParams))
                .ToPagedList(filtersParams.PageNumber, filtersParams.PageSize);

        public async Task<Order> CreateOrder(Order order)
        {
            if (order == null)
                throw new ServerException("Order does not exists");

            return await database.OrderRepository.Insert(order, false)
                ? order
                : throw new DatabaseException("Inserting order failed");
        }

        public async Task<SetApprovalStateResult> SetOrderApprovalState(string orderId, StateType approvalState,
            string adminNote = null)
        {
            if (!await rolesService.IsPermitted(httpContextReader.CurrentUserId, Constants.AllAdminsRoles))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            var order = await database.OrderRepository.FindById(orderId) ??
                        throw new EntityNotFoundException("Order not found");

            if (order.IsOrderReviewed)
                throw new NoPermissionsException("This order has already been reviewed");

            order.SetApprovalState(approvalState);
            order.SetAdminNote(adminNote);
            order.Review();

            return await database.OrderRepository.Update(order)
                ? new SetApprovalStateResult(true, order.User.Id)
                : throw new DataException();
        }
    }
}