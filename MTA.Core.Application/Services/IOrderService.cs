using System.Threading.Tasks;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IOrderService : IReadOnlyOrderService
    {
        Task<Order> CreateOrder(Order order);

        Task<SetApprovalStateResult> SetOrderApprovalState(string orderId, StateType approvalState, string adminNote = null);
    }
}