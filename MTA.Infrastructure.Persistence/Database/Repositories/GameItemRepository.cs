using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class GameItemRepository : Repository<GameItem>, IGameItemRepository
    {
        public GameItemRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<IEnumerable<GameItem>> GetCharacterItems(int characterId)
            => await Query(new SqlBuilder()
                .Select<GameItem>("lastused", "odcisk")
                .From(Table)
                .Where("type").Equals.Append(1)
                .And.Append("owner").Equals.Append(characterId)
                .Build());

        public async Task<IEnumerable<GameItem>> GetAccountItems(IEnumerable<int> charactersIds)
            => await Query(new SqlBuilder()
                .Select<GameItem>("lastused", "odcisk")
                .From(Table)
                .WhereIn("owner", charactersIds.ToArray())
                .And.Append("type").Equals.Append(1)
                .Build());
    }
}