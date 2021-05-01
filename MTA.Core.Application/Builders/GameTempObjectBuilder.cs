using System;
using System.Globalization;
using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class GameTempObjectBuilder : IGameTempObjectBuilder
    {
        private readonly GameTempObject gameTempObject = new GameTempObject();
        private readonly NumberFormatInfo culture = CultureInfo.InvariantCulture.NumberFormat;

        public IGameTempObjectBuilder SetModel(string model)
        {
            gameTempObject.SetModel(Convert.ToInt32(model));
            return this;
        }

        public IGameTempObjectBuilder SetPosition(string x, string y, string z)
        {
            gameTempObject.SetPosition(float.Parse(x, culture), float.Parse(y, culture), float.Parse(z, culture));
            return this;
        }

        public IGameTempObjectBuilder SetRotation(string x, string y, string z)
        {
            gameTempObject.SetRotation(float.Parse(x, culture), float.Parse(y, culture), float.Parse(z, culture));
            return this;
        }

        public IGameTempObjectBuilder SetComment(string comment)
        {
            gameTempObject.SetComment(comment);
            return this;
        }

        public IGameTempObjectBuilder SetDimension(int interiorId)
        {
            gameTempObject.SetDimension(interiorId);
            return this;
        }

        public IGameTempObjectBuilder SetInterior(int estateInterior)
        {
            gameTempObject.SetInterior(estateInterior);
            return this;
        }

        public GameTempObject Build() => gameTempObject;
    }
}