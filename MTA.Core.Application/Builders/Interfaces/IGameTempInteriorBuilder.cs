using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IGameTempInteriorBuilder : IBuilder<GameTempInterior>
    {
        IGameTempInteriorBuilder SetInteriorId(int interiorId);
        IGameTempInteriorBuilder SetPosition(string x, string y, string z);
        IGameTempInteriorBuilder SetEstateInterior(int estateInterior);
        IGameTempInteriorBuilder UploadedBy(int uploadedBy);
    }
}