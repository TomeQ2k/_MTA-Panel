using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record MarkAsReadNotificationRequest : IRequest<MarkAsReadNotificationResponse>
    {
        public string NotificationId { get; init; }
    }

    public class MarkAsReadNotificationRequestValidator : AbstractValidator<MarkAsReadNotificationRequest>
    {
        public MarkAsReadNotificationRequestValidator()
        {
            RuleFor(x => x.NotificationId).NotNull();
        }
    }
}