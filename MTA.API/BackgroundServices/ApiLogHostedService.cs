using System;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Logging;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.BackgroundServices
{
    internal class ApiLogHostedService : ServerHostedService
    {
        public ApiLogHostedService(IServiceProvider services) : base(services)
        {
            TimeInterval = Constants.ApiLogHostedServiceTimeInMinutes;
        }

        public override async void Callback(object state)
        {
            using (var scope = services.CreateScope())
            {
                var logCleaner = scope.ServiceProvider.GetRequiredService<ILogCleaner>();

                await logCleaner.ClearLogs();

                Log.Information($"Logs older than {Constants.ApiLogTrashTimeInDays} days were cleared");
                base.Callback(state);
            }
        }
    }
}