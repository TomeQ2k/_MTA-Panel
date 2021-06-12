using System;
using System.Threading.Tasks;

namespace MTA.Core.Application.SignalR.Hubs
{
    public class NotifierHub : HubClient
    {
        public NotifierHub(IConnectionManager connectionManager, HubNamesDictionary hubNamesDictionary) : base(
            connectionManager, hubNamesDictionary)
        {
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            await connectionManager.CloseConnection(hubNamesDictionary[typeof(NotifierHub)]);

            await base.OnDisconnectedAsync(exception);
        }
    }
}