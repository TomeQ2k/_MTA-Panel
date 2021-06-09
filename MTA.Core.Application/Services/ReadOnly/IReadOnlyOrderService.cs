using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyOrderService
    {
        Task<PagedList<Order>> GetOrders(OrderFiltersParams filtersParams);
    }
}