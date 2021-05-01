using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IOrderTransactionBuilder : IBuilder<OrderTransaction>
    {
        IOrderTransactionBuilder SetTransaction(string transactionId);
        IOrderTransactionBuilder SetAmount(decimal amount);
        IOrderTransactionBuilder DonatedBy(string email, string username);
    }
}