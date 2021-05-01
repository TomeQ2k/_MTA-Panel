using System.Globalization;
using System.IO;
using System.Reflection;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using MTA.API.AppConfigs;
using MTA.Core.Application.Logging;
using MTA.Core.Application.Mapper;
using MTA.Core.Application.SignalR;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            DapperAppConfig.ConfigureSqlMapper();

            services.ConfigureAuthentication(Configuration);
            services.ConfigureAuthorization();

            services.ConfigureMvc()
                .ConfigureFluentValidation();

            services.ConfigureIpRateLimit(Configuration);

            services.ConfigureDatabase(Configuration);

            services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });

            services.AddOptions();
            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.ConfigureCors();

            services.AddMediatR(Assembly.Load("MTA.Core.Application"));

            #region services

            services.ConfigureScopedServices();
            services.ConfigureSingletonServices();

            #endregion

            services.ConfigureCaching();

            services.ConfigureSettings(Configuration);

            services.AddAutoMapper(typeof(MapperProfile));

            services.AddDirectoryBrowser();

            services.ConfigureServerHostedServices();

            services.ConfigureSwagger();

            services.AddSignalR();

            services.AddSingleton<HubNamesDictionary>(s => HubNamesDictionary.Build());
            services.AddSingleton<LogActionPermissionDictionary>(s => LogActionPermissionDictionary.Build());
            services.AddSingleton<LogKeyWordsDictionary>(s => LogKeyWordsDictionary.Build());

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MTA.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(Constants.CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotifierHub>("/api/hub/notifier");
            });

            app.UseFileServer(new FileServerOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(env.WebRootPath, @"files")),
                RequestPath = new PathString("/files"),
                EnableDirectoryBrowsing = true
            });

            var cultureInfo = new CultureInfo("pl-PL");
            cultureInfo.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            cultureInfo.DateTimeFormat.LongDatePattern = "d MMMM yyyy";
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}