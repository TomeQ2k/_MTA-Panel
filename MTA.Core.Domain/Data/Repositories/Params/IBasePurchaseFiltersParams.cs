using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IBasePurchaseFiltersParams
    {
        DateSortType SortType { get; init; }
    }
}