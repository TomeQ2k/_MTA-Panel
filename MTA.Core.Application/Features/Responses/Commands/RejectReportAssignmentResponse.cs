using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record RejectReportAssignmentResponse : BaseResponse
    {
        public bool IsAccepted { get; init; }

        public RejectReportAssignmentResponse(Error error = null) : base(error)
        {
        }
    }
}