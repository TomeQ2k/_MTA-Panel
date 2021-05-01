using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Factories;
using MTA.Core.Application.Factories.Interfaces;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Infrastructure.Persistence.Logging;
using MTA.Infrastructure.Shared.Services;

namespace MTA.API.AppConfigs
{
    public static class SingletonServicesAppConfig
    {
        public static IServiceCollection ConfigureSingletonServices(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, SendGridEmailSender>();
            services.AddSingleton<IFilesManager, FilesManager>();
            services.AddSingleton<IEmailTemplateGenerator, EmailTemplateGenerator>();
            services.AddSingleton<ICaptchaService, CaptchaService>();
            services.AddSingleton<IHttpContextService, HttpContextService>();
            services.AddSingleton<IHttpContextWriter, HttpContextService>();
            services.AddSingleton<IHttpContextReader, HttpContextService>();
            services.AddSingleton<IMtaManager, MtaManager>();
            services.AddSingleton<ILogCleaner, ApiLogCleaner>();
            services.AddSingleton<IXmlReader, XmlReader>();

            services.AddSingleton<IReadOnlyFilesManager, FilesManager>();
            services.AddSingleton<IReadOnlyEmailTemplateGenerator, EmailTemplateGenerator>();
            
            services.AddSingleton<IPaypalClientFactory, PaypalClientFactory>();

            return services;
        }
    }
}