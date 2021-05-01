using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class RestoreDefaultInteriorCommand :
        IRequestHandler<RestoreDefaultInteriorRequest, RestoreDefaultInteriorResponse>
    {
        private readonly IMtaManager mtaManager;
        private readonly IReadOnlyUserService userService;
        private readonly IReadOnlyCharacterService characterService;
        private readonly IHttpContextReader httpContextReader;

        public RestoreDefaultInteriorCommand(IMtaManager mtaManager, IReadOnlyUserService userService,
            IReadOnlyCharacterService characterService, IHttpContextReader httpContextReader)
        {
            this.mtaManager = mtaManager;
            this.userService = userService;
            this.characterService = characterService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<RestoreDefaultInteriorResponse> Handle(RestoreDefaultInteriorRequest request,
            CancellationToken cancellationToken)
        {
            var user = await userService.GetUserWithCharacters(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            if (await characterService.HasAnyCharacterEstate(user.Characters, request.EstateId) == null)
                throw new NoPermissionsException("You are not owner of this estate");

            //TODO: Call LUA script
            return new RestoreDefaultInteriorResponse();
        }
    }
}