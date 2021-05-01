using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class AdminActionBuilder : IAdminActionBuilder
    {
        private readonly AdminAction adminAction = new AdminAction();

        public IAdminActionBuilder SetReason(string reason)
        {
            adminAction.SetReason(reason);
            return this;
        }

        public IAdminActionBuilder SetUserId(int userId)
        {
            adminAction.SetUserId(userId);
            return this;
        }

        public IAdminActionBuilder SetCharacterId(int characterId)
        {
            adminAction.SetCharacterId(characterId);
            return this;
        }

        public IAdminActionBuilder SetAdminId(int adminId)
        {
            adminAction.SetAdminId(adminId);
            return this;
        }

        public IAdminActionBuilder SetAction(AdminActionType actionType)
        {
            adminAction.SetAction(actionType);
            return this;
        }

        public IAdminActionBuilder SetDuration(int duration)
        {
            adminAction.SetDuration(duration);
            return this;
        }

        public AdminAction Build() => this.adminAction;
    }
}