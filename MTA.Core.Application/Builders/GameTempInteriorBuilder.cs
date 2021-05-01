using System.Globalization;
using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class GameTempInteriorBuilder : IGameTempInteriorBuilder
    {
        private readonly GameTempInterior gameTempInterior = new GameTempInterior();
        private readonly NumberFormatInfo culture = CultureInfo.InvariantCulture.NumberFormat;

        public IGameTempInteriorBuilder SetInteriorId(int interiorId)
        {
            gameTempInterior.SetInteriorId(interiorId);
            return this;
        }

        public IGameTempInteriorBuilder SetPosition(string x, string y, string z)
        {
            gameTempInterior.SetPosition(float.Parse(x, culture), float.Parse(y, culture), float.Parse(z, culture));
            return this;
        }

        public IGameTempInteriorBuilder SetEstateInterior(int estateInterior)
        {
            gameTempInterior.SetEstateInterior(estateInterior);
            return this;
        }

        public IGameTempInteriorBuilder UploadedBy(int uploadedBy)
        {
            gameTempInterior.SetUploadedBy(uploadedBy);
            return this;
        }

        public GameTempInterior Build() => this.gameTempInterior;
    }
}