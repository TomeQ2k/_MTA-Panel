using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Helpers;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Persistence.Logging
{
    public class LogReaderHelper : ILogReaderHelper
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;
        private readonly LogActionPermissionDictionary logActionPermissionDictionary;

        public IConfiguration Configuration { get; }

        public LogReaderHelper(IDatabase database, IHttpContextReader httpContextReader,
            LogActionPermissionDictionary logActionPermissionDictionary, IConfiguration configuration)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
            this.logActionPermissionDictionary = logActionPermissionDictionary;
            Configuration = configuration;
        }

        public async Task<IEnumerable<LogAction>> GetAllowedLogActions()
        {
            var currentUser = await database.UserRepository.FindById(httpContextReader.CurrentUserId) ??
                              throw new AuthException("User not authorized");

            var currentUserRoles = new[]
            {
                RoleDictionary.FindRoleTypeByUserRole(new("admin", currentUser.AdminRole)),
                RoleDictionary.FindRoleTypeByUserRole(new("supporter", currentUser.SupporterRole)),
                RoleDictionary.FindRoleTypeByUserRole(new("mapper", currentUser.MapperRole)),
                RoleDictionary.FindRoleTypeByUserRole(new("vct", currentUser.VctRole)),
                RoleDictionary.FindRoleTypeByUserRole(new("scripter", currentUser.ScripterRole))
            }.Where(ur => ur != 0);

            if (Configuration.IsDev(httpContextReader.CurrentUserId))
                currentUserRoles = currentUserRoles.Append(RoleType.Owner);

            var allowedLogActions = GetLogActionsFromDictionary(currentUserRoles);

            return allowedLogActions.Any()
                ? allowedLogActions
                : throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);
        }

        #region private

        private IEnumerable<LogAction> GetLogActionsFromDictionary(IEnumerable<RoleType> roleTypes)
        {
            IEnumerable<LogAction> logActionsFromDictionary = new List<LogAction>();

            foreach (var roleType in roleTypes)
                if (logActionPermissionDictionary.ContainsKey(roleType))
                    logActionsFromDictionary = logActionsFromDictionary.Concat(logActionPermissionDictionary[roleType]);
                else
                    throw new ServerException("This role has no permissions to filter logs");

            return logActionsFromDictionary.Distinct();
        }

        #endregion
    }
}