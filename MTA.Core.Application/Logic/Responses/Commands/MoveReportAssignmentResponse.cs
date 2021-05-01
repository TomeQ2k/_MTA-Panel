using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record MoveReportAssignmentResponse : BaseResponse
    {
        public MoveReportAssignmentResponse(Error error = null) : base(error)
        {
        }
    }
}