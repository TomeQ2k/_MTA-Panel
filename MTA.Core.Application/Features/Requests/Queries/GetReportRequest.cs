using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetReportRequest : IRequest<GetReportResponse>
    {
        public string ReportId { get; init; }
    }

    public class GetReportRequestValidator : AbstractValidator<GetReportRequest>
    {
        public GetReportRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
        }
    }
}