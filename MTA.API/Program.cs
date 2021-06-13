using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTA.Core.Common.Helpers;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace MTA.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter())
                .WriteTo.File(new CompactJsonFormatter(), Constants.LogFilesPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Seq("http://localhost:5000")
                .CreateLogger();

            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    Log.Information("Application started...");

                    Log.Information("Application initialized");

                    host.Run();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Application terminated unexpectedly...");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .UseSerilog();
    }
}