using Microsoft.Extensions.DependencyInjection;
using MTA.API.BackgroundServices;

namespace MTA.API.AppConfigs
{
    public static class ServerHostedServicesAppConfig
    {
        public static IServiceCollection ConfigureServerHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<StatsHostedService>();
            services.AddHostedService<TokenHostedService>();
            services.AddHostedService<ReportHostedService>();
            services.AddHostedService<ApiLogHostedService>();
            services.AddHostedService<DatabaseRestoreHostedService>();

            return services;
        }
    }
}