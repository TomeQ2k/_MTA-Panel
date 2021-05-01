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
    public class
        ToggleBlockCharacterCommand : IRequestHandler<ToggleBlockCharacterRequest, ToggleBlockCharacterResponse>
    {
        private readonly ICharacterManager characterManager;
        private readonly IHttpContextReader httpContextReader;
        private readonly INotifier notifier;
        private readonly IHubManager<NotifierHub> hubManager;
        private readonly IMapper mapper;

        public ToggleBlockCharacterCommand(ICharacterManager characterManager, IHttpContextReader httpContextReader,
            INotifier notifier, IHubManager<NotifierHub> hubManager, IMapper mapper)
        {
            this.characterManager = characterManager;
            this.httpContextReader = httpContextReader;
            this.notifier = notifier;
            this.hubManager = hubManager;
            this.mapper = mapper;
        }

        public async Task<ToggleBlockCharacterResponse> Handle(ToggleBlockCharacterRequest request,
            CancellationToken cancellationToken)
        {
            var blockCharacterResult = await characterManager.ToggleBlockCharacter(request.CharacterId);

            if (blockCharacterResult.IsSucceeded)
            {
                var notification = await notifier.Push(
                    $"Character {blockCharacterResult.CharacterName} has been blocked",
                    blockCharacterResult.AccountId);

                await hubManager.Invoke(SignalrActions.NOTIFICATION_RECEIVED, blockCharacterResult.AccountId,
                    mapper.Map<NotificationDto>(notification));

                return (ToggleBlockCharacterResponse) new ToggleBlockCharacterResponse
                    {IsBlocked = blockCharacterResult.IsBlocked}.LogInformation(
                    $"Admin #{httpContextReader.CurrentUserId} blocked character #{request.CharacterId}");
            }

            throw new CrudException("Toggling block character status failed");
        }
    }
}