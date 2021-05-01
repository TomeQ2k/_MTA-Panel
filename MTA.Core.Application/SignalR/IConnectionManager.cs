using System.Threading.Tasks;

namespace MTA.Core.Application.SignalR
{
    public interface IConnectionManager
    {
        Task<bool> StartConnection(string connectionId, string hubName);
        Task<bool> CloseConnection(string hubName);

        Task<string> GetConnectionId(int userId, string hubName);
    }
}