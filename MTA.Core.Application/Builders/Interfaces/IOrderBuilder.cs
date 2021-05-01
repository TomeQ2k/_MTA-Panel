using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IOrderBuilder : IBuilder<Order>
    {
        IOrderBuilder SetOperation(PremiumOperation operation);
        IOrderBuilder SetCost(int cost);
        IOrderBuilder SetUser(int userId);
        IOrderBuilder SetEstate(int estateId);
        IOrderBuilder SetCharacter(int characterId);
        IOrderBuilder SetApprovalState(StateType stateType);
        IOrderBuilder SetAdminNote(string adminNote);
    }
}