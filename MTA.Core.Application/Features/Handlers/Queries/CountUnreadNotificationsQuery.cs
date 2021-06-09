using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class CountUnreadNotificationsQuery
        : IRequestHandler<CountUnreadNotificationsRequest, CountUnreadNotificationsResponse>
    {
        private readonly IReadOnlyNotifier notifier;

        public CountUnreadNotificationsQuery(IReadOnlyNotifier notifier)
        {
            this.notifier = notifier;
        }

        public async Task<CountUnreadNotificationsResponse> Handle(CountUnreadNotificationsRequest request, CancellationToken cancellationToken)
            => new CountUnreadNotificationsResponse {UnreadNotificationsCount = await notifier.CountUnreadNotifications()};
    }
}