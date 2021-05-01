using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AcceptReportAssignmentRequest : IRequest<AcceptReportAssignmentResponse>
    {
        public string ReportId { get; init; }
    }

    public class AcceptReportAssignmentRequestValidator : AbstractValidator<AcceptReportAssignmentRequest>
    {
        public AcceptReportAssignmentRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
        }
    }
}