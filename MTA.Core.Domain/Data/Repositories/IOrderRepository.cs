using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrders(IOrderFiltersParams filtersParams);
    }
}