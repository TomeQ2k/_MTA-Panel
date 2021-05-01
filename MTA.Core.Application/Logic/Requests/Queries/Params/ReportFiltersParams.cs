using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Logic.Requests.Queries.Params
{
    public abstract record ReportFiltersParams : PaginationRequest, IReportFiltersParams
    {
        public ReportCategoryType CategoryType { get; init; } = ReportCategoryType.All;
        public ReportStatusType StatusType { get; init; } = ReportStatusType.All;
        public DateSortType SortType { get; init; } = DateSortType.Ascending;
        public int? AdminId { get; init; }
    }
}