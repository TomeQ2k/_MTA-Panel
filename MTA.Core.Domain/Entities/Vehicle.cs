using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class Vehicle : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("model")] public int ModelId { get; protected set; }
        [Column("plate")] public string Plate { get; protected set; }
        [Column("owner")] public int OwnerId { get; protected set; }
        [Column("lastUsed")] public DateTime LastUsed { get; protected set; }
        [Column("protected_until")] public DateTime? ProtectedUntil { get; protected set; }

        public VehiclesShop VehiclesShop { get; protected set; }

        public void SetOwnerId(int ownerId) => OwnerId = ownerId;
        public void SetVehiclesShop(VehiclesShop vehiclesShop) => VehiclesShop = vehiclesShop;

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