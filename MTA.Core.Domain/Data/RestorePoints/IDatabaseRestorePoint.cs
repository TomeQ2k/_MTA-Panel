using System;
using System.Threading.Tasks;
using MTA.Core.Domain.Data.RestorePoints.Params;

namespace MTA.Core.Domain.Data.RestorePoints
{
    public interface IDatabaseRestorePoint : IDisposable
    {
        IDatabaseRestoreParams RestoreParams { get; }

        Task<bool> Restore();

        IDatabaseRestorePoint Enqueue();

        IDatabaseRestorePoint CreateRestoreParams(IDatabaseRestoreParams restoreParams);
        IDatabaseRestorePoint EnqueueToConnectionDatabaseRestorePoints(string connectionId);
    }
}