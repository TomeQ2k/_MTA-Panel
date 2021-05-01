using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class CleanAccountCommand : IRequestHandler<CleanAccountRequest, CleanAccountResponse>
    {
        private readonly IUserManager userManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public CleanAccountCommand(IUserManager userManager, IHttpContextReader httpContextReader, INotifier notifier,
            IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<CleanAccountResponse> Handle(CleanAccountRequest request, CancellationToken cancellationToken)
        {
            if (await userManager.CleanAccount(request.UserId))
            {
                var notification = await notifier.Push(
                    $"Admin has cleaned your account",
                    request.UserId);

                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, request.UserId,
                    mapper.Map<NotificationDto>(notification));

                return (CleanAccountResponse) new CleanAccountResponse().LogInformation(
                    $"Admin #{httpContextReader.CurrentUserId} cleaned account of user #{request.UserId}");
            }

            throw new CrudException($"Cleaning account #{request.UserId} failed");
        }
    }
}