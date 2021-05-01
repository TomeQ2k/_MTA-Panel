using System.Linq;
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
    public class RestoreDefaultSkinCommand : IRequestHandler<RestoreDefaultSkinRequest, RestoreDefaultSkinResponse>
    {
        private readonly IMtaManager mtaManager;
        private readonly IReadOnlyUserService userService;
        private readonly IHttpContextReader httpContextReader;

        public RestoreDefaultSkinCommand(IMtaManager mtaManager, IReadOnlyUserService userService,
            IHttpContextReader httpContextReader)
        {
            this.mtaManager = mtaManager;
            this.userService = userService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<RestoreDefaultSkinResponse> Handle(RestoreDefaultSkinRequest request,
            CancellationToken cancellationToken)
        {
            var user = await userService.GetUserWithCharacters(httpContextReader.CurrentUserId) ??
                       throw new EntityNotFoundException("User not found");

            if (!user.Characters.Any(c => c.Id == request.CharacterId))
                throw new NoPermissionsException("You are not owner of this character");

            //TODO: Call LUA script

            return new RestoreDefaultSkinResponse();
        }
    }
}