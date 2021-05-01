using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IGameTempObjectBuilder : IBuilder<GameTempObject>
    {
        IGameTempObjectBuilder SetModel(string model);
        IGameTempObjectBuilder SetPosition(string x, string y, string z);
        IGameTempObjectBuilder SetRotation(string x, string y, string z);
        IGameTempObjectBuilder SetComment(string comment);
        IGameTempObjectBuilder SetDimension(int interiorId);
        IGameTempObjectBuilder SetInterior(int estateInterior);
    }
}