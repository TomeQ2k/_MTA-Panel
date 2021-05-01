using System.Threading.Tasks;
using MTA.Core.Application.Models;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IOrderTransactionService
    {
        Task<OrderTransaction> CreateOrderTransaction(string transactionId, decimal amount,
            EmailUsernameTuple emailUsernameTuple);
    }
}