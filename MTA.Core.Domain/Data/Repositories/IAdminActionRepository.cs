using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IAdminActionRepository : IRepository<AdminAction>
    {
        Task<IEnumerable<AdminAction>> GetAdminActionsWithUserAndCharacterNamesByActionAndUserId(
            AdminActionType actionType, int userId);

        Task<IEnumerable<AdminAction>> GetAdminActionsAsUserBans(int userId);
    }
}