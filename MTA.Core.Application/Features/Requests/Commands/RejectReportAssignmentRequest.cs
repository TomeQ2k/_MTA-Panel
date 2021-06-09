using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record RejectReportAssignmentRequest : IRequest<RejectReportAssignmentResponse>
    {
        public string ReportId { get; init; }
    }

    public class RejectReportRequestValidator : AbstractValidator<RejectReportAssignmentRequest>
    {
        
    }
}