using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IFactionRepository : IRepository<Faction>
    {
        Task<IEnumerable<Faction>> GetTopFactionsByBankBalance(int top = Constants.TopStatsLimit);
    }
}