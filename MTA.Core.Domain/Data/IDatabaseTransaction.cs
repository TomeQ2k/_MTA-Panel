using System.Transactions;

namespace MTA.Core.Domain.Data
{
    public interface IDatabaseTransaction
    {
        TransactionScope Transaction { get; }
    }
}