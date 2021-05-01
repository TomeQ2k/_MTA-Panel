using System.Data;

namespace MTA.Core.Domain.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection Connection { get; }
    }
}