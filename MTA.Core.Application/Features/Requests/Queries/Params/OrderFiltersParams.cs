using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Features.Requests.Queries.Params
{
    public abstract record OrderFiltersParams : PaginationRequest, IOrderFiltersParams
    {
        public string Username { get; init; }
        public StateStatusType StateStatusType { get; init; } = StateStatusType.All;

        public DateSortType SortType { get; init; }
    }
}