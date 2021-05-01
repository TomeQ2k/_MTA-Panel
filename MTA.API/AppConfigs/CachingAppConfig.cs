using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Caching;
using MTA.Core.Application.Caching.Memory;
using MTA.Infrastructure.Persistence.Caching;
using MTA.Infrastructure.Persistence.Caching.Memory;

namespace MTA.API.AppConfigs
{
    public static class CachingAppConfig
    {
        public static IServiceCollection ConfigureCaching(this IServiceCollection services)
        {
            services.AddMemoryCache();

            services.AddScoped(typeof(IMemoryCacheService<>), typeof(MemoryCacheService<>));

            services.AddScoped<IStatsMemoryCacheService, StatsMemoryCacheService>();

            return services;
        }
    }
}