using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddReportSubscriberRequest : IRequest<AddReportSubscriberResponse>
    {
        public string ReportId { get; init; }
        public int UserId { get; init; }
    }

    public class AddReportSubscriberRequestValidator : AbstractValidator<AddReportSubscriberRequest>
    {
        public AddReportSubscriberRequestValidator()
        {
            RuleFor(x => x.ReportId).NotNull();
            RuleFor(x => x.UserId).NotNull();
        }
    }
}