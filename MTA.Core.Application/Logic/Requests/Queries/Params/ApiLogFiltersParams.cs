using System;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Repositories.Params;

namespace MTA.Core.Application.Logic.Requests.Queries.Params
{
    public abstract record ApiLogFiltersParams : PaginationRequest, IApiLogFiltersParams
    {
        public DateTime Date { get; init; } = DateTime.Now.AddDays(-1);

        public string Message { get; init; }
        public string Level { get; init; }
        public string RequestMethod { get; init; }
        public string RequestPath { get; init; }
        public int? StatusCode { get; init; }
        public DateTime? MinTime { get; init; }
        public DateTime? MaxTime { get; init; }

        public ApiLogSortType SortType { get; init; }

        public ApiLogFiltersParams()
        {
            PageSize = Constants.LogsPageSize;
        }
    }
}