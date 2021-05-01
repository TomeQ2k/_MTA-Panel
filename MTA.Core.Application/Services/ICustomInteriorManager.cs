using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface ICustomInteriorManager
    {
        (IEnumerable<GameTempObject>, GameTempInterior) InitGameTempObjectsAndInteriors(Estate estate,
            PremiumFile premiumFile);

        Task ExecuteAddCustomInterior(PremiumFile premiumFile, IEnumerable<GameTempObject> gameTempObjects,
            GameTempInterior gameTempInterior);
    }
}