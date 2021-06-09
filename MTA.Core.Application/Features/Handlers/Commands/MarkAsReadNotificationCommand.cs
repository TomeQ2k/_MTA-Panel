using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class MarkAsReadNotificationCommand
        : IRequestHandler<MarkAsReadNotificationRequest, MarkAsReadNotificationResponse>
    {
        private readonly INotifier notifier;

        public MarkAsReadNotificationCommand(INotifier notifier)
        {
            this.notifier = notifier;
        }

        public async Task<MarkAsReadNotificationResponse> Handle(MarkAsReadNotificationRequest request,
            CancellationToken cancellationToken)
            => await notifier.MarkAsRead(request.NotificationId)
                ? new MarkAsReadNotificationResponse()
                : throw new CrudException("Marking notification as read failed");
    }
}