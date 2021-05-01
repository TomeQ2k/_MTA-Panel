using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Data.RestorePoints;

namespace MTA.Core.Domain.Data
{
    public interface IDatabaseRestorer
    {
        Queue<IDatabaseRestorePoint> DatabaseRestorePoints { get; }
        Dictionary<string, IDatabaseRestorePoint> ConnectionDatabaseRestorePoints { get; }

        Task<bool> Execute();

        void EnqueueFromConnectionDatabaseRestorePoints(string connectionId);
    }
}