using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record CloseReportResponse : BaseResponse
    {
        public CloseReportResponse(Error error = null) : base(error)
        {
        }
    }
}