using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record ArchiveReportRequest : IRequest<ArchiveReportResponse>
    {
        public string ReportId { get; init; }
    }

    public class ArchiveReportRequestValidator : AbstractValidator<ArchiveReportRequest>
    {
        public ArchiveReportRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
        }
    }
}