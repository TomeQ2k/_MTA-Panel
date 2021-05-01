using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetNotificationsQuery : IRequestHandler<GetNotificationsRequest, GetNotificationsResponse>
    {
        private readonly IReadOnlyNotifier notifier;
        private readonly IMapper mapper;

        public GetNotificationsQuery(IReadOnlyNotifier notifier, IMapper mapper)
        {
            this.notifier = notifier;
            this.mapper = mapper;
        }

        public async Task<GetNotificationsResponse> Handle(GetNotificationsRequest request,
            CancellationToken cancellationToken)
        {
            var notifications = await notifier.GetNotifications();

            return new GetNotificationsResponse
                {Notifications = mapper.Map<IEnumerable<NotificationDto>>(notifications)};
        }
    }
}