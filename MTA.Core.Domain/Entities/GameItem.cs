using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class GameItem : EntityModel
    {
        [Column("index", true)] public int Id { get; protected set; }
        [Column("type")] public int Type { get; protected set; }
        [Column("owner")] public int OwnerId { get; protected set; }
        [Column("itemID")] public int ItemId { get; protected set; }
        [Column("itemValue")] public string ItemValue { get; protected set; }
        [Column("protected")] public int Protected { get; protected set; }
        [Column("lastused")] public DateTime LastUsed { get; protected set; }
        [Column("odcisk")] public string Imprint { get; protected set; } = "brak";

        public void SetOwnerId(int ownerId) => OwnerId = ownerId;

        public bool IsProtected => Protected == 1;
    }
}