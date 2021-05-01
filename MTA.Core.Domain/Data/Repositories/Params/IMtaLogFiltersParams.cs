using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IMtaLogFiltersParams
    {
        TimeAgoType MinTimeAgo { get; init; }
        TimeAgoType MaxTimeAgo { get; init; }

        string Content { get; init; }
        ContentFilterType ContentFilterType { get; init; }

        LogAction[] Actions { get; init; }

        string SourceAffected { get; init; }
        SourceAffectedFilterType SourceAffectedFilterType { get; init; }
        SourceAffectedLogType SourceAffectedLogType { get; init; }

        DateSortType SortType { get; set; }
    }
}