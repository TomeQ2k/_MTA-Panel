using System.Threading.Tasks;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public abstract class BaseStatsService<TStats> : IStatsService<TStats> where TStats : class, new()
    {
        protected readonly IDatabase database;

        public BaseStatsService(IDatabase database)
        {
            this.database = database;
        }

        public abstract Task<TStats> SelectStats();
    }
}