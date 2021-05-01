using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyAdminActionService
    {
        Task<IEnumerable<AdminAction>> GetAdminActionsByActionAndUserId(AdminActionType actionType);
        Task<IEnumerable<AdminAction>> GetAdminActionsAsUserBans();
    }
}