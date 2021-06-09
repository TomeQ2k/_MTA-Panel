using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Features.Requests.Queries.Params
{
    public abstract record UserReportFiltersParams : PaginationRequest, IUserReportFiltersParams
    {
        public ReportCategoryType CategoryType { get; init; } = ReportCategoryType.All;
        public ReportStatusType StatusType { get; init; } = ReportStatusType.All;
        public DateSortType SortType { get; init; } = DateSortType.Ascending;
    }
}