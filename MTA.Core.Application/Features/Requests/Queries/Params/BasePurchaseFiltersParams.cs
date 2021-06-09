using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Features.Requests.Queries.Params
{
    public abstract record BasePurchaseFiltersParams : PaginationRequest, IBasePurchaseFiltersParams
    {
        public DateSortType SortType { get; init; }
    }
}