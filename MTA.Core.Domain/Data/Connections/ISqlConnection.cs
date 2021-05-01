using System.Data;

namespace MTA.Core.Domain.Data.Connections
{
    public interface ISqlConnection
    {
        IDbConnection Connection { get; }
    }
}