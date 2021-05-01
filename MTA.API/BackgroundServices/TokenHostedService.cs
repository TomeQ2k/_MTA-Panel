using System;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using Serilog;

namespace MTA.API.BackgroundServices
{
    internal class TokenHostedService : ServerHostedService
    {
        public TokenHostedService(IServiceProvider services) : base(services)
        {
            TimeInterval = Constants.TokenHostedServiceTimeInMinutes;
        }

        public override async void Callback(object state)
        {
            using (var scope = services.CreateScope())
            {
                var tokenCleaner = scope.ServiceProvider.GetRequiredService<ITokenCleaner>();

                await tokenCleaner.ClearTokens();

                Log.Information("Expired tokens were cleared");
                base.Callback(state);
            }
        }
    }
}