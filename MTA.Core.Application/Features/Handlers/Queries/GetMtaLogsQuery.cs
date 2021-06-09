using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetMtaLogsQuery : IRequestHandler<GetMtaLogsRequest, GetMtaLogsResponse>
    {
        private readonly ILogReader logReader;
        private readonly IRolesService rolesService;
        private readonly IReadOnlyCharacterService characterService;
        private readonly IReadOnlyUserService userService;
        private readonly IHttpContextReader httpContextReader;

        public GetMtaLogsQuery(ILogReader logReader, IRolesService rolesService,
            IReadOnlyCharacterService characterService, IReadOnlyUserService userService,
            IHttpContextReader httpContextReader)
        {
            this.logReader = logReader;
            this.rolesService = rolesService;
            this.characterService = characterService;
            this.userService = userService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<GetMtaLogsResponse> Handle(GetMtaLogsRequest request,
            CancellationToken cancellationToken)
        {
            if (request.Actions.All(l => l == LogAction.PhoneSms) &&
                request.SourceAffectedLogType != SourceAffectedLogType.PhoneNumber)
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            foreach (var logAction in request.Actions)
                if (!await LogActionPermissionSmartEnum.FromValue((int) logAction)
                    .IsPermitted(rolesService, httpContextReader.CurrentUserId))
                    throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            IEnumerable<SourceAffectedModel> sourceAffectedModels = request.SourceAffectedLogType switch
            {
                SourceAffectedLogType.Account => (await userService.GetUsersByUsername(request.SourceAffected))
                    .Select(u => new SourceAffectedModel(u.Id, u.Username, SourceAffectedLogType.Account)),
                SourceAffectedLogType.Character => (await characterService.GetCharactersByCharactername(
                        request.SourceAffected))
                    .Select(c => new SourceAffectedModel(c.Id, c.Name, SourceAffectedLogType.Character)),
                _ => new List<SourceAffectedModel>()
            };

            var mtaLogs = await logReader.GetMtaLogsFromDatabase(request, sourceAffectedModels);

            return new GetMtaLogsResponse {MtaLogs = mtaLogs};
        }
    }
}