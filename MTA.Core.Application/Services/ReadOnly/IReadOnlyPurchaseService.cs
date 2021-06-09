using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyPurchaseService
    {
        Task<PagedList<Purchase>> GetUserPurchases(BasePurchaseFiltersParams filtersParams);
        Task<PagedList<Purchase>> GetPurchasesByAdmin(AdminPurchaseFiltersParams filtersParams);
    }
}