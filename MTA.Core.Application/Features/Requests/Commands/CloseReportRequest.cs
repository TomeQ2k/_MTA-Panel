using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record CloseReportRequest : IRequest<CloseReportResponse>
    {
        public string ReportId { get; init; }
    }

    public class CloseReportRequestValidator : AbstractValidator<CloseReportRequest>
    {
        public CloseReportRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
        }
    }
}