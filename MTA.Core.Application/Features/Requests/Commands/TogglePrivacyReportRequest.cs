using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record TogglePrivacyReportRequest : IRequest<TogglePrivacyReportResponse>
    {
        public string ReportId { get; init; }
    }

    public class TogglePrivacyReportRequestValidator : AbstractValidator<TogglePrivacyReportRequest>
    {
        public TogglePrivacyReportRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
        }
    }
}