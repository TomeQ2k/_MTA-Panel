using System;
using System.Globalization;
using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class GameTempObject : EntityModel
    {
        [Column("id", true)] public int Id { get; protected set; }
        [Column("model")] public int ModelId { get; protected set; }
        [Column("posX")] public float PosX { get; protected set; }
        [Column("posY")] public float PosY { get; protected set; }
        [Column("posZ")] public float PosZ { get; protected set; }
        [Column("rotX")] public float RotX { get; protected set; }
        [Column("rotY")] public float RotY { get; protected set; }
        [Column("rotZ")] public float RotZ { get; protected set; }
        [Column("interior")] public int Interior { get; protected set; }
        [Column("dimension")] public int Dimension { get; protected set; }
        [Column("comment")] public string Comment { get; protected set; }
        [Column("solid")] public int Solid { get; protected set; }
        [Column("doublesided")] public int DoubleSided { get; protected set; }
        [Column("scale")] public float Scale { get; protected set; }
        [Column("breakable")] public int Breakable { get; protected set; }
        [Column("alpha")] public int Alpha { get; protected set; }

        public void SetModel(int modelId) => ModelId = modelId;
        public void SetPosition(float x, float y, float z) => (PosX, PosY, PosZ) = (x, y, z);
        public void SetRotation(float x, float y, float z) => (RotX, RotY, RotZ) = (x, y, z);
        public void SetComment(string comment) => Comment = comment;
        public void SetDimension(int interiorId) => Dimension = interiorId;
        public void SetInterior(int estateInterior) => Interior = estateInterior;
        public void SetSolid(int solid) => Solid = solid;

        public void SetDoublesided(int doubleSided) => DoubleSided = doubleSided;

        public void SetScale(string scale) => Scale = float.Parse(scale, CultureInfo.InvariantCulture.NumberFormat);

        public void SetBreakable(int breakable) => Breakable = breakable;

        public void SetAlpha(string alpha) => Alpha = Convert.ToInt32(alpha);
    }
}