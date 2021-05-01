using System.Threading.Tasks;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class TempDatabaseCleaner : ITempDatabaseCleaner
    {
        private readonly IDatabase database;

        public TempDatabaseCleaner(IDatabase database)
        {
            this.database = database;
        }

        public async Task ClearGameTempObjectsAndInteriors()
        {
            var (gameTempObjects, gameTempInteriors) = (await database.GameTempObjectRepository.GetAll(limit: 1000),
                await database.GameTempInteriorRepository.GetAll(limit: 1000));

            await database.GameTempObjectRepository.DeleteRange(gameTempObjects);
            await database.GameTempInteriorRepository.DeleteRange(gameTempInteriors);
        }
    }
}