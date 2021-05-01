using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace MTA.API.BackgroundServices
{
    internal abstract class ServerHostedService : IHostedService, IDisposable
    {
        protected readonly IServiceProvider services;

        public int TimeInterval { get; protected set; }
        public int HostedServiceDelayInSeconds { get; protected set; } = 10;

        private Timer timer;

        public ServerHostedService(IServiceProvider services)
        {
            this.services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Log.Information($"{this.GetType().Name}: Background server hosted service started...");

            timer = new Timer(Callback, null, TimeSpan.FromSeconds(HostedServiceDelayInSeconds),
                TimeSpan.FromMinutes(TimeInterval));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Information($"{this.GetType().Name}: Background server hosted service stopped...");

            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public virtual void Callback(object state)
        {
            Log.Information($"{this.GetType().Name}: Background server hosted service invoked");
        }
    }
}