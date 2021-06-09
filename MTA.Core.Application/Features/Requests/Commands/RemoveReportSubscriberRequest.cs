using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record RemoveReportSubscriberRequest : IRequest<RemoveReportSubscriberResponse>
    {
        public string ReportId { get; init; }
        public int UserId { get; init; }
    }

    public class RemoveReportSubscriberRequestValidator : AbstractValidator<RemoveReportSubscriberRequest>
    {
        public RemoveReportSubscriberRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
            RuleFor(x => x.UserId).NotNull();
        }
    }
}