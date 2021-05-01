using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record CloseReportResponse : BaseResponse
    {
        public CloseReportResponse(Error error = null) : base(error)
        {
        }
    }
}