using System.Collections.Generic;
using System.Threading.Tasks;

namespace MTA.Core.Domain.Data
{
    public interface IDatabaseExecutor
    {
        Task<bool> Execute(SqlQuery query);

        Task<IEnumerable<TValue>> SelectQuery<TValue>(SqlQuery query);
        Task<TValue> SelectQueryFirst<TValue>(SqlQuery query);

        IDatabaseTransaction BeginTransaction();
    }
}