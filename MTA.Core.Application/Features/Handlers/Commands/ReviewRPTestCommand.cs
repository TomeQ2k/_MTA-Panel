using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Application.SignalR.Hubs;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class ReviewRPTestCommand : IRequestHandler<ReviewRPTestRequest, ReviewRPTestResponse>
    {
        private readonly IRPTestManager rpTestManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public ReviewRPTestCommand(IRPTestManager rpTestManager, IHttpContextReader httpContextReader,
            INotifier notifier, IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.rpTestManager = rpTestManager;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<ReviewRPTestResponse> Handle(ReviewRPTestRequest request, CancellationToken cancellationToken)
        {
            var result = await rpTestManager.ReviewRPTest(request.ApplicationId, request.Note, request.StateType);
            if (result.IsSucceeded)
            {
                var notification = await notifier.Push(
                    "Status of your RP test has been changed",
                    result.UserId);

                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, result.UserId,
                    mapper.Map<NotificationDto>(notification));

                return (ReviewRPTestResponse) new ReviewRPTestResponse().LogInformation(
                    $"Reviewer #{httpContextReader.CurrentUserId} review application test #{request.ApplicationId} and set state to: {(int) request.StateType}");
            }

            throw new ServerException("Processing RP test failed");
        }
    }
}