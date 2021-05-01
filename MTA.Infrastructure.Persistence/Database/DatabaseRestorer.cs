using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.RestorePoints;

namespace MTA.Infrastructure.Persistence.Database
{
    public class DatabaseRestorer : IDatabaseRestorer
    {
        public Queue<IDatabaseRestorePoint> DatabaseRestorePoints { get; private set; } =
            new Queue<IDatabaseRestorePoint>();

        public Dictionary<string, IDatabaseRestorePoint> ConnectionDatabaseRestorePoints { get; private set; } =
            new Dictionary<string, IDatabaseRestorePoint>();

        public async Task<bool> Execute()
        {
            bool allRestored = true;

            while (DatabaseRestorePoints.Any())
            {
                var currentRestored = await DatabaseRestorePoints.Peek().Restore();
                allRestored = allRestored && currentRestored;

                if (currentRestored)
                    DatabaseRestorePoints.Dequeue();
            }

            return allRestored;
        }

        public void EnqueueFromConnectionDatabaseRestorePoints(string connectionId)
        {
            IDatabaseRestorePoint databaseRestorePoint = default;
            this.ConnectionDatabaseRestorePoints.TryGetValue(connectionId, out databaseRestorePoint);

            if (databaseRestorePoint != default)
                this.DatabaseRestorePoints.Enqueue(databaseRestorePoint);
        }
    }
}