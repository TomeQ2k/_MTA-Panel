using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IArchivedReportFiltersParams
    {
        ReportCategoryType CategoryType { get; init; }
        DateSortType SortType { get; init; }
        int? AdminId { get; init; }
    }
}