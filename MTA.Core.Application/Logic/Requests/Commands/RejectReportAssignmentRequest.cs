using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record RejectReportAssignmentRequest : IRequest<RejectReportAssignmentResponse>
    {
        public string ReportId { get; init; }
    }

    public class RejectReportRequestValidator : AbstractValidator<RejectReportAssignmentRequest>
    {
        
    }
}