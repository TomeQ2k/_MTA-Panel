using System;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using Serilog;

namespace MTA.API.BackgroundServices
{
    internal class DatabaseRestoreHostedService : ServerHostedService
    {
        public DatabaseRestoreHostedService(IServiceProvider services) : base(services)
        {
            TimeInterval = Constants.DatabaseRestoreHostedServiceTimeInMinutes;
        }

        public override async void Callback(object state)
        {
            Log.Information("Database restoring started...");

            using (var scope = services.CreateScope())
            {
                var databaseRestorer = scope.ServiceProvider.GetRequiredService<IDatabaseRestorer>();

                if (await databaseRestorer.Execute())
                    Log.Information("Database restore has been completed");
                else
                    Log.Warning("During database restoring an error occurred");
                
                base.Callback(state);
            }
        }
    }
}