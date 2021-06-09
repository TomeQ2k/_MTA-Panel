using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Features.Requests.Queries.Params
{
    public abstract record MtaLogFiltersParams : PaginationRequest, IMtaLogFiltersParams
    {
        public TimeAgoType MinTimeAgo { get; init; }
        public TimeAgoType MaxTimeAgo { get; init; }

        public string Content { get; init; }
        public ContentFilterType ContentFilterType { get; init; }

        public LogAction[] Actions { get; init; }

        public string SourceAffected { get; init; }
        public SourceAffectedFilterType SourceAffectedFilterType { get; init; }
        public SourceAffectedLogType SourceAffectedLogType { get; init; }

        public DateSortType SortType { get; set; } = DateSortType.Descending;

        public MtaLogFiltersParams()
        {
            PageSize = Constants.LogsPageSize;
        }
    }
}