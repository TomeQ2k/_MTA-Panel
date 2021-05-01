using System.Threading.Tasks;

namespace MTA.Core.Application.Services
{
    public interface IStatsService<TStats> where TStats : class, new()
    {
        Task<TStats> SelectStats();
    }
}