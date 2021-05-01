using System.Transactions;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Persistence.Database
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        public TransactionScope Transaction => new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
    }
}