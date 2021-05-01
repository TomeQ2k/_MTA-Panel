using System.Collections.Generic;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetMtaLogsResponse : BaseResponse
    {
        public IEnumerable<MtaLogModel> MtaLogs { get; init; }

        public GetMtaLogsResponse(Error error = null) : base(error)
        {
        }
    }
}