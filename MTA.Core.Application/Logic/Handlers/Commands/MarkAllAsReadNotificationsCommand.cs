using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class MarkAllAsReadNotificationsCommand
        : IRequestHandler<MarkAllAsReadNotificationsRequest, MarkAllAsReadNotificationsResponse>
    {
        private readonly INotifier notifier;

        public MarkAllAsReadNotificationsCommand(INotifier notifier)
        {
            this.notifier = notifier;
        }

        public async Task<MarkAllAsReadNotificationsResponse> Handle(MarkAllAsReadNotificationsRequest request,
            CancellationToken cancellationToken)
            => await notifier.MarkAllAsRead()
                ? new MarkAllAsReadNotificationsResponse()
                : throw new CrudException("Marking all notifications as read failed");
    }
}