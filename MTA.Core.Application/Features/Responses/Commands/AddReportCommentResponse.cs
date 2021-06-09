using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record AddReportCommentResponse : BaseResponse
    {
        public ReportCommentDto ReportComment { get; init; }

        public AddReportCommentResponse(Error error = null) : base(error)
        {
        }
    }
}