using Microsoft.AspNetCore.SignalR;

namespace MTA.Core.Application.SignalR
{
    public abstract class HubClient : Hub
    {
        protected readonly IConnectionManager connectionManager;
        protected readonly HubNamesDictionary hubNamesDictionary;

        public HubClient(IConnectionManager connectionManager, HubNamesDictionary hubNamesDictionary)
        {
            this.connectionManager = connectionManager;
            this.hubNamesDictionary = hubNamesDictionary;
        }

        public string GetConnectionId() => Context.ConnectionId;
    }
}