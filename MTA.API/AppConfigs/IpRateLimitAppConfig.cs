using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Common.Helpers;

namespace MTA.API.AppConfigs
{
    public static class IpRateLimitAppConfig
    {
        public static IServiceCollection ConfigureIpRateLimit(this IServiceCollection services,
            IConfiguration configuration)
            => services.Configure<IpRateLimitOptions>(configuration.GetSection(AppSettingsKeys.IpRateLimitingSection))
                .AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>()
                .AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>()
                .AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }
}