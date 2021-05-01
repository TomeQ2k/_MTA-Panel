using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IConnectionRepository : IRepository<Connection>
    {
        Task<Connection> FindUserHubConnection(int userId, string hubName);

        Task<IEnumerable<Connection>> GetUserHubConnections(int userId, string hubName);
    }
}