using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.ServicesUtils;
using MTA.Core.Application.SmartEnums;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class RolesService : IRolesService
    {
        private readonly IDatabase database;

        public IConfiguration Configuration { get; }

        public RolesService(IDatabase database, IConfiguration configuration)
        {
            this.database = database;
            Configuration = configuration;
        }

        public async Task<bool> AdmitRole(User user, RoleType roleType)
        {
            if (user == null)
                throw new EntityNotFoundException("User not found");

            var query = RoleTypeSmartEnum.FromValue((int) roleType).Admit(user.Id);

            return await database.Execute(query);
        }

        public async Task<bool> AdmitRole(int userId, RoleType roleType)
        {
            var query = RoleTypeSmartEnum.FromValue((int) roleType).Admit(userId);

            return await database.Execute(query);
        }

        public async Task<bool> RevokeRole(User user, RoleType roleType)
        {
            if (user == null)
                throw new EntityNotFoundException("User not found");

            var query = RoleTypeSmartEnum.FromValue((int) roleType).Revoke(user.Id);

            return await database.Execute(query);
        }

        public async Task<bool> RevokeRole(int userId, RoleType roleType)
        {
            var query = RoleTypeSmartEnum.FromValue((int) roleType).Revoke(userId);

            return await database.Execute(query);
        }

        public async Task<bool> IsPermitted(User user, params RoleType[] roleTypes)
        {
            if (user == null)
                throw new EntityNotFoundException("User not found");

            if (Configuration.IsDev(user.Id))
                return true;

            var query = RolesServiceUtils.BuildIsPermittedQuery(user.Id, roleTypes);

            var result = await database.SelectQueryFirst<IsPermittedResult>(query)
                         ?? throw new DatabaseException();

            return result.IsPermitted;
        }

        public async Task<bool> IsPermitted(int userId, params RoleType[] roleTypes)
        {
            if (Configuration.IsDev(userId))
                return true;

            var query = RolesServiceUtils.BuildIsPermittedQuery(userId, roleTypes);

            var result = await database.SelectQueryFirst<IsPermittedResult>(query)
                         ?? throw new DatabaseException();

            return result.IsPermitted;
        }
    }
}