using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class ConnectionRepository : Repository<Connection>, IConnectionRepository
    {
        public ConnectionRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<Connection> FindUserHubConnection(int userId, string hubName)
            => await QueryFirst(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("userId").Equals.Append(userId)
                .And.Append("hubName").Equals.Append($"'{hubName}'")
                .Build());

        public async Task<IEnumerable<Connection>> GetUserHubConnections(int userId, string hubName)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("userId").Equals.Append(userId)
                .And.Append("hubName").Equals.Append($"'{hubName}'")
                .Build());
    }
}