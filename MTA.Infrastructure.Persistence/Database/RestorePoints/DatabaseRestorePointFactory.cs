using System;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.RestorePoints;

namespace MTA.Infrastructure.Persistence.Database.RestorePoints
{
    public class DatabaseRestorePointFactory<TDatabaseRestorePoint> where TDatabaseRestorePoint : IDatabaseRestorePoint
    {
        public static TDatabaseRestorePoint Create(IDatabaseRestorer databaseRestorer, IServiceProvider services)
            => (TDatabaseRestorePoint) Activator.CreateInstance(typeof(TDatabaseRestorePoint),
                new object[] {databaseRestorer, services});
    }
}