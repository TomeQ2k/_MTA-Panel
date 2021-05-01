using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class FactionRepository : Repository<Faction>, IFactionRepository
    {
        public FactionRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<IEnumerable<Faction>> GetTopFactionsByBankBalance(int top = Constants.TopStatsLimit)
            => await Query(new SqlBuilder()
                .Select().Append(",")
                .Open
                .SelectCount()
                .From(RepositoryDictionary.FindTable(typeof(ICharacterRepository))).As("p")
                .Where("p.faction_id").Equals.Append("f.id").Close.As("WorkersCount")
                .From(Table).As("f")
                .OrderBy("f.bankbalance", OrderByType.Descending)
                .Limit(top)
                .Build());
    }
}