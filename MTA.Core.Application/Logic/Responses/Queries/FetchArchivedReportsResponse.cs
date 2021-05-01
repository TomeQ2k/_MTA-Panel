using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record FetchArchivedReportsResponse : BaseResponse
    {
        public IEnumerable<ReportListDto> Reports { get; init; }

        public FetchArchivedReportsResponse(Error error = null) : base(error)
        {
        }
    }
}