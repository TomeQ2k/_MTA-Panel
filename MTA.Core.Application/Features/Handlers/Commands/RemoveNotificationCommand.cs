using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class RemoveNotificationCommand : IRequestHandler<RemoveNotificationRequest, RemoveNotificationResponse>
    {
        private readonly INotifier notifier;

        public RemoveNotificationCommand(INotifier notifier)
        {
            this.notifier = notifier;
        }

        public async Task<RemoveNotificationResponse> Handle(RemoveNotificationRequest request, CancellationToken cancellationToken)
            => await notifier.RemoveNotification(request.NotificationId)
                ? new RemoveNotificationResponse()
                : throw new CrudException("Removing notification failed");
    }
}