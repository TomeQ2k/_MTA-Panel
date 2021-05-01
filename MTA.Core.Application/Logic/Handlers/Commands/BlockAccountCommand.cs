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
    public class BlockAccountCommand : IRequestHandler<BlockAccountRequest, BlockAccountResponse>
    {
        private readonly IUserManager userManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public BlockAccountCommand(IUserManager userManager, IHttpContextReader httpContextReader, INotifier notifier,
            IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<BlockAccountResponse> Handle(BlockAccountRequest request, CancellationToken cancellationToken)
        {
            if (await userManager.BlockAccount(request))
            {
                var notification = await notifier.Push(
                    $"Your account has been blocked",
                    request.AccountId);

                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, request.AccountId,
                    mapper.Map<NotificationDto>(notification));

                return (BlockAccountResponse) new BlockAccountResponse().LogInformation(
                    $"Admin #{httpContextReader} has blocked user #{request.AccountId}");
            }

            throw new CrudException("Blocking user account failed");
        }
    }
}