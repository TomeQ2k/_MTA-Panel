using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class OrderTransactionBuilder : IOrderTransactionBuilder
    {
        private readonly OrderTransaction orderTransaction = new OrderTransaction();

        public IOrderTransactionBuilder SetTransaction(string transactionId)
        {
            orderTransaction.SetTransactionId(transactionId);
            return this;
        }

        public IOrderTransactionBuilder SetAmount(decimal amount)
        {
            orderTransaction.SetAmount(amount);
            return this;
        }

        public IOrderTransactionBuilder DonatedBy(string email, string username)
        {
            orderTransaction.SetEmail(email);
            orderTransaction.SetUsername(username);
            return this;
        }

        public OrderTransaction Build() => this.orderTransaction;
    }
}