using System;
using Microsoft.Extensions.Caching.Memory;

namespace MTA.Core.Application.Helpers
{
    public static class CachingConstants
    {
        private static TimeSpan memoryCacheSlidingExpiration = TimeSpan.FromDays(1);

        public static MemoryCacheEntryOptions MemoryCacheEntryOptions
            => new MemoryCacheEntryOptions()
                .SetSlidingExpiration(memoryCacheSlidingExpiration);
    }
}