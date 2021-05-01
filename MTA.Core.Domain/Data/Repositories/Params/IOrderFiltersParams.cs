using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IOrderFiltersParams
    {
        string Username { get; init; }
        StateStatusType StateStatusType { get; init; }

        DateSortType SortType { get; init; }
    }
}