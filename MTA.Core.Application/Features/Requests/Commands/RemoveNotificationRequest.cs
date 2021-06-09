using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record RemoveNotificationRequest : IRequest<RemoveNotificationResponse>
    {
        public string NotificationId { get; init; }
    }

    public class RemoveNotificationRequestValidator : AbstractValidator<RemoveNotificationRequest>
    {
        public RemoveNotificationRequestValidator()
        {
            RuleFor(x => x.NotificationId).NotNull();
        }
    }
}