using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Logic.Requests.Queries.Params
{
    public abstract record AdminPurchaseFiltersParams : BasePurchaseFiltersParams, IAdminPurchaseFiltersParams
    {
        public string Username { get; init; }
    }
}