using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class AdminActionService : IAdminActionService
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public AdminActionService(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<IEnumerable<AdminAction>> GetAdminActionsByActionAndUserId(AdminActionType actionType)
            => await database.AdminActionRepository.GetAdminActionsWithUserAndCharacterNamesByActionAndUserId(
                actionType, httpContextReader.CurrentUserId);

        public async Task<IEnumerable<AdminAction>> GetAdminActionsAsUserBans()
            => await database.AdminActionRepository.GetAdminActionsAsUserBans(httpContextReader.CurrentUserId);

        public async Task<AdminAction> InsertAdminAction(AdminAction adminAction)
        {
            if (adminAction == null)
                throw new ServerException("Admin action to insert does not exist");

            return await database.AdminActionRepository.Insert(adminAction)
                ? adminAction
                : throw new DatabaseException();
        }
    }
}