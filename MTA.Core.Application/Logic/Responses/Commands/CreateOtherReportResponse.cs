using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record CreateOtherReportResponse : BaseResponse
    {
        public ReportDto Report { get; init; }

        public CreateOtherReportResponse(Error error = null) : base(error)
        {
        }
    }
}