using System;
using MTA.Core.Common.Enums;

namespace MTA.Core.Domain.Data.Repositories.Params
{
    public interface IApiLogFiltersParams
    {
        DateTime Date { get; init; }

        string Message { get; init; }
        string Level { get; init; }
        string RequestMethod { get; init; }
        string RequestPath { get; init; }
        int? StatusCode { get; init; }
        DateTime? MinTime { get; init; }
        DateTime? MaxTime { get; init; }

        ApiLogSortType SortType { get; init; }
    }
}