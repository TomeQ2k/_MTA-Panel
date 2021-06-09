using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class AddCreditsCommand : IRequestHandler<AddCreditsRequest, AddCreditsResponse>
    {
        private readonly IUserManager userManager;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly IMapper mapper;

        public AddCreditsCommand(IUserManager userManager, INotifier notifier, IHubManager<NotifierHub> hubManager,
            IHttpContextReader httpContextReader, IMapper mapper)
        {
            this.userManager = userManager;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.httpContextReader = httpContextReader;
            this.mapper = mapper;
        }

        public async Task<AddCreditsResponse> Handle(AddCreditsRequest request, CancellationToken cancellationToken)
        {
            var result = await userManager.AddCredits(request.Credits, request.UserId);

            var notification = await notifier.Push(
                $"Account credits amount {(request.Credits > 0 ? "increased by" : "decreased by")} {Math.Abs(request.Credits)}, current balance: {result.CurrentCreditsCount}",
                request.UserId);

            await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, request.UserId,
                mapper.Map<NotificationDto>(notification));

            return (AddCreditsResponse) new AddCreditsResponse {CreditsCount = result.CurrentCreditsCount}
                .LogInformation(
                    $"Admin #{httpContextReader.CurrentUserId} added {request.Credits} to user #{request.UserId}");
        }
    }
}