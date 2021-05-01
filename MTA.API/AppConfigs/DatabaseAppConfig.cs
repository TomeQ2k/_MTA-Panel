using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Connections;
using MTA.Core.Domain.Data.RestorePoints;
using MTA.Infrastructure.Persistence.Database;
using MTA.Infrastructure.Persistence.Database.Connections;
using MTA.Infrastructure.Persistence.Database.RestorePoints;

namespace MTA.API.AppConfigs
{
    public static class DatabaseAppConfig
    {
        public static IServiceCollection ConfigureDatabase(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<ISqlConnection, MySqlConnection>(_ =>
                new MySqlConnection(configuration.GetConnectionString(AppSettingsKeys.ConnectionString)));

            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

            services.AddScoped<IDatabase, Database>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<IDatabaseRestorer, DatabaseRestorer>();

            services.AddSingleton<IPremiumCreditsDatabaseRestorePoint>(s =>
                DatabaseRestorePointFactory<PremiumCreditsDatabaseRestorePoint>.Create(
                    s.GetService<IDatabaseRestorer>(), s));

            return services;
        }
    }
}