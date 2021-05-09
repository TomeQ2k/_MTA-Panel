using Microsoft.AspNetCore.Builder;
using Serilog;

namespace MTA.API.AppConfigs
{
    public static class LoggingAppConfig
    {
        public static void ConfigureLogging(this IApplicationBuilder app)
            => app.UseSerilogRequestLogging();
    }
}