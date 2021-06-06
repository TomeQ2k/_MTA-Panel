using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using MTA.Core.Application.Caching;
using MTA.Core.Application.Helpers;

namespace MTA.Infrastructure.Persistence.Caching
{
    public class MemoryCacheService<T> : IMemoryCacheService<T>
    {
        protected readonly IMemoryCache memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public virtual Task<T> Get(string key)
        {
            T entry = default;

            memoryCache.TryGetValue(key, out entry);

            return Task.FromResult(entry);
        }

        public void Set(string key, T entry, MemoryCacheEntryOptions options = null)
            => memoryCache.Set(key, entry, options ?? CachingConstants.MemoryCacheEntryOptions);

        public void Remove(string key)
            => memoryCache.Remove(key);
    }
}