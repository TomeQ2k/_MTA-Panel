using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Queries
{
    public record GetReportResponse : BaseResponse
    {
        public ReportDto Report { get; init; }

        public GetReportResponse(Error error = null) : base(error)
        {
        }
    }
}