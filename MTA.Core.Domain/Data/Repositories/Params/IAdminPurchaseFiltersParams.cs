namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IAdminPurchaseFiltersParams : IBasePurchaseFiltersParams
    {
        string Username { get; init; }
    }
}