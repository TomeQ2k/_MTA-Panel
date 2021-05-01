using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IReportFiltersParams
    {
        ReportCategoryType CategoryType { get; init; }
        ReportStatusType StatusType { get; init; }
        DateSortType SortType { get; init; }
        int? AdminId { get; init; }
    }
}