using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Estate : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("name")] public string Name { get; protected set; }
        [Column("type")] public int Type { get; set; }
        [Column("owner")] public int OwnerId { get; protected set; }
        [Column("cost")] public int Cost { get; protected set; }
        [Column("lastused")] public DateTime? LastUsed { get; protected set; }
        [Column("protected_until")] public DateTime? ProtectedUntil { get; protected set; }
        [Column("interior")] public int InteriorId { get; protected set; }

        public void SetOwnerId(int ownerId) => OwnerId = ownerId;

        public void Protect(int days)
        {
            if (ProtectedUntil < DateTime.Now)
                ProtectedUntil = null;

            ProtectedUntil = ProtectedUntil == null
                ? DateTime.Now.AddDays(days)
                : ProtectedUntil.Value.AddDays(days);
        }
    }
}