using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class ClearAllNotificationsCommand 
        : IRequestHandler<ClearAllNotificationsRequest, ClearAllNotificationsResponse>
    {
        private readonly INotifier notifier;

        public ClearAllNotificationsCommand(INotifier notifier)
        {
            this.notifier = notifier;
        }

        public async Task<ClearAllNotificationsResponse> Handle(ClearAllNotificationsRequest request, CancellationToken cancellationToken)
            => await notifier.ClearAllNotifications()
                ? new ClearAllNotificationsResponse()
                : throw new CrudException("Clearing all notifications failed");
    }
}