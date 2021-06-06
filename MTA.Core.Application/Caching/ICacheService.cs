using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace MTA.Core.Application.Caching
{
    public interface ICacheService<T>
    {
        Task<T> Get(string key);

        void Set(string key, T entry, MemoryCacheEntryOptions options = null);

        void Remove(string key);
    }
}