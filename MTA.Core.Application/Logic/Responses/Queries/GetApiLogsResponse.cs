using System.Collections.Generic;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetApiLogsResponse : BaseResponse
    {
        public IEnumerable<ApiLogModel> ApiLogs { get; init; }

        public GetApiLogsResponse(Error error = null) : base(error)
        {
        }
    }
}