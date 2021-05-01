using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
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