using System;
using System.Threading.Tasks;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Core.Domain.Data.RestorePoints.Params;
using Serilog;

namespace MTA.Infrastructure.Persistence.Database.RestorePoints
{
    public abstract class DatabaseRestorePoint : IDatabaseRestorePoint
    {
        protected readonly IDatabaseRestorer databaseRestorer;
        protected readonly IServiceProvider services;

        public DatabaseRestorePoint(IDatabaseRestorer databaseRestorer, IServiceProvider services)
        {
            this.databaseRestorer = databaseRestorer;
            this.services = services;
        }

        public IDatabaseRestoreParams RestoreParams { get; protected set; }

        public virtual Task<bool> Restore()
        {
            Log.Information($"{this.GetType().FullName}: Database restore point has been restored...");

            return Task.FromResult(true);
        }

        public IDatabaseRestorePoint Enqueue()
        {
            databaseRestorer.DatabaseRestorePoints.Enqueue(this);

            Log.Information($"{this.GetType().FullName}: Database restore point has been enqueued...");

            return this;
        }

        public IDatabaseRestorePoint CreateRestoreParams(IDatabaseRestoreParams restoreParams)
        {
            RestoreParams = restoreParams;
            return this;
        }

        public IDatabaseRestorePoint EnqueueToConnectionDatabaseRestorePoints(string connectionId)
        {
            databaseRestorer.ConnectionDatabaseRestorePoints.Remove(connectionId);
            databaseRestorer.ConnectionDatabaseRestorePoints.Add(connectionId, this);

            return this;
        }

        public void Dispose()
        {
        }
    }
}