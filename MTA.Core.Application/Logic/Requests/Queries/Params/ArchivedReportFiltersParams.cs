using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Logic.Requests.Queries.Params
{
    public abstract record ArchivedReportFiltersParams : PaginationRequest, IArchivedReportFiltersParams
    {
        public ReportCategoryType CategoryType { get; init; }
        public DateSortType SortType { get; init; }
        public int? AdminId { get; init; }
    }
}