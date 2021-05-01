using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IAdminActionBuilder : IBuilder<AdminAction>
    {
        IAdminActionBuilder SetReason(string reason);
        IAdminActionBuilder SetUserId(int userId);
        IAdminActionBuilder SetCharacterId(int characterId);
        IAdminActionBuilder SetAdminId(int adminId);
        IAdminActionBuilder SetAction(AdminActionType actionType);
        IAdminActionBuilder SetDuration(int duration);
    }
}