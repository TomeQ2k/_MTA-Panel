using System.Threading.Tasks;

namespace MTA.Core.Application.SignalR
{
    public interface IHubManager<THub> where THub : HubClient
    {
        Task Invoke(string actionName, int clientId, params object[] values);
        Task InvokeToAll(string actionName, params object[] values);
    }
}