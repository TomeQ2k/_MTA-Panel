using System.Data;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Connections;

namespace MTA.Infrastructure.Persistence.Database
{
    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly ISqlConnection connection;

        public SqlConnectionFactory(ISqlConnection connection)
            => (this.connection) = (connection);

        public IDbConnection Connection => connection.Connection;
    }
}