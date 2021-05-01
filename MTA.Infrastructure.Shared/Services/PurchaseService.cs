using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public PurchaseService(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<PagedList<Purchase>> GetUserPurchases(BasePurchaseFiltersParams filtersParams)
            => (await database.PurchaseRepository.GetPurchasesWithUsers(filtersParams.SortType,
                httpContextReader.CurrentUserId)).ToPagedList(filtersParams.PageNumber, filtersParams.PageSize);

        public async Task<PagedList<Purchase>> GetPurchasesByAdmin(AdminPurchaseFiltersParams filtersParams)
            => (await database.PurchaseRepository.GetPurchasesWithUsersByAdmin(filtersParams)).ToPagedList(
                filtersParams.PageNumber, filtersParams.PageSize);

        public async Task<Purchase> CreatePurchase(string name, int userId, int? cost = null)
        {
            var purchase = Purchase.Create(name, userId, cost);

            return await database.PurchaseRepository.Insert(purchase)
                ? purchase
                : throw new DatabaseException();
        }
    }
}