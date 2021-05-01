using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.SignalR
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public ConnectionManager(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<bool> StartConnection(string connectionId, string hubName)
        {
            var connectionsToDelete =
                (await database.ConnectionRepository.GetUserHubConnections(httpContextReader.CurrentUserId,
                    hubName.ToUpper())).Select(c => new ColumnValue("connectionId", c.ConnectionId));

            await database.ConnectionRepository.DeleteRangeByColumn(connectionsToDelete.ToArray());

            var connection = Connection.Create(httpContextReader.CurrentUserId, connectionId, hubName);

            return await database.ConnectionRepository.Insert(connection, false);
        }

        public async Task<bool> CloseConnection(string hubName)
        {
            var connectionsToDelete =
                (await database.ConnectionRepository.GetUserHubConnections(httpContextReader.CurrentUserId,
                    hubName.ToUpper())).Select(c => new ColumnValue("connectionId", c.ConnectionId));

            return connectionsToDelete.Any()
                ? await database.ConnectionRepository.DeleteRangeByColumn(connectionsToDelete.ToArray())
                : true;
        }

        public async Task<string> GetConnectionId(int userId, string hubName)
            => (await database.ConnectionRepository
                .FindUserHubConnection(userId, hubName))?.ConnectionId;
    }
}