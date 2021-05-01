using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class OrderTransactionService : IOrderTransactionService
    {
        private readonly IDatabase database;

        public OrderTransactionService(IDatabase database)
        {
            this.database = database;
        }

        public async Task<OrderTransaction> CreateOrderTransaction(string transactionId, decimal amount,
            EmailUsernameTuple emailUsernameTuple)
        {
            var orderTransaction = new OrderTransactionBuilder()
                .SetTransaction(transactionId)
                .SetAmount(amount)
                .DonatedBy(emailUsernameTuple.Email, emailUsernameTuple.Username)
                .Build();

            return await database.OrderTransactionRepository.Insert(orderTransaction)
                ? orderTransaction
                : throw new DatabaseException("Creating order transaction failed");
        }
    }
}