using System;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class GameTempInterior : EntityModel
    {
        [Column("primaryId", true)] public int Id { get; protected set; }
        [Column("id")] public int InteriorId { get; protected set; }
        [Column("posX")] public float PosX { get; protected set; }
        [Column("posY")] public float PosY { get; protected set; }
        [Column("posZ")] public float PosZ { get; protected set; }
        [Column("interior")] public int EstateInterior { get; protected set; }
        [Column("uploaded_by")] public int UploadedBy { get; protected set; }
        [Column("uploaded_at")] public DateTime? UploadedAt { get; protected set; } = DateTime.Now;
        [Column("amount_paid")] public int? AmountPaid { get; protected set; }

        public void SetInteriorId(int interiorId) => InteriorId = interiorId;

        public void SetPosition(float x, float y, float z) => (PosX, PosY, PosZ) = (x, y, z);

        public void SetEstateInterior(int estateInterior) => EstateInterior = estateInterior;

        public void SetUploadedBy(int uploadedBy) => UploadedBy = uploadedBy;
    }
}