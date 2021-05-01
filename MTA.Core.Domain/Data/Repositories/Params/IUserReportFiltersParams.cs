using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IUserReportFiltersParams
    {
        public ReportCategoryType CategoryType { get; init; }
        public ReportStatusType StatusType { get; init; }
        public DateSortType SortType { get; init; }
    }
}