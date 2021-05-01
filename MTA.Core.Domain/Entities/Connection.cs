using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Connection : EntityModel
    {
        [Column("userId")] public int UserId { get; protected set; }
        [Column("connectionId")] public string ConnectionId { get; protected set; }
        [Column("dateEstablished")] public DateTime DateEstablished { get; protected set; } = DateTime.Now;
        [Column("hubName")] public string HubName { get; protected set; }

        public static Connection Create(int userId, string connId, string hubName) => new Connection
        {
            UserId = userId,
            ConnectionId = connId,
            HubName = hubName.ToUpper()
        };
    }
}