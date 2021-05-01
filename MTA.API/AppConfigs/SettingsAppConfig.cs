using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Settings;

namespace MTA.API.AppConfigs
{
    public static class SettingsAppConfig
    {
        public static IServiceCollection ConfigureSettings(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
            services.Configure<CaptchaSettings>(configuration.GetSection(nameof(CaptchaSettings)));
            services.Configure<MtaServerSettings>(configuration.GetSection(nameof(MtaServerSettings)));
            services.Configure<PaypalSettings>(configuration.GetSection(nameof(PaypalSettings)));
            
            return services;
        }
    }
}