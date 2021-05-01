using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IPurchaseRepository : IRepository<Purchase>
    {
        Task<IEnumerable<Purchase>> GetPurchasesWithUsers(DateSortType sortType, int userId);
        Task<IEnumerable<Purchase>> GetPurchasesWithUsersByAdmin(IAdminPurchaseFiltersParams filtersParams);
    }
}