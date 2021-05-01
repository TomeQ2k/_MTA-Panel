using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
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