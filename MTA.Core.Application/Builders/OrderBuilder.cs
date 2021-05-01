using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class OrderBuilder : IOrderBuilder
    {
        private readonly Order order = new Order();

        public IOrderBuilder SetOperation(PremiumOperation operation)
        {
            order.SetOperation(operation);
            return this;
        }

        public IOrderBuilder SetCost(int cost)
        {
            order.SetCost(cost);
            return this;
        }

        public IOrderBuilder SetUser(int userId)
        {
            order.SetUserId(userId);
            return this;
        }

        public IOrderBuilder SetEstate(int estateId)
        {
            order.SetEstateId(estateId);
            return this;
        }

        public IOrderBuilder SetCharacter(int characterId)
        {
            order.SetCharacterId(characterId);
            return this;
        }

        public IOrderBuilder SetApprovalState(StateType stateType)
        {
            order.SetApprovalState(stateType);
            return this;
        }

        public IOrderBuilder SetAdminNote(string adminNote)
        {
            order.SetAdminNote(adminNote);
            return this;
        }

        public Order Build() => order;
    }
}