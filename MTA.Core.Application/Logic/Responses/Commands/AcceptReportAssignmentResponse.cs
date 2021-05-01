using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record AcceptReportAssignmentResponse : BaseResponse
    {
        public bool IsAccepted { get; init; }

        public AcceptReportAssignmentResponse(Error error = null) : base(error)
        {
        }
    }
}