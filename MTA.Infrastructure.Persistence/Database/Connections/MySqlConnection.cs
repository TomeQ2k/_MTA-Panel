using System.Data;
using MTA.Core.Domain.Data.Connections;

namespace MTA.Infrastructure.Persistence.Database.Connections
{
    public class MySqlConnection : ISqlConnection
    {
        public MySqlConnection(string connectionString)
            => (Connection) = (new MySql.Data.MySqlClient.MySqlConnection(connectionString));

        public IDbConnection Connection { get; init; }
    }
}