using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Results;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class ServerStatsService : BaseStatsService<ServerStatsResult>
    {
        public ServerStatsService(IDatabase database) : base(database)
        {
        }

        public async override Task<ServerStatsResult> SelectStats()
        {
            var stats = new ServerStatsResult();

            var query = new SqlBuilder()
                .Select(false)
                .Append(CreateInternalSelectCountQuery(nameof(stats.AccountsCount), database.UserRepository.Table))
                .Append(",")
                .Append(CreateInternalSelectCountQuery(nameof(stats.CharactersCount),
                    database.CharacterRepository.Table)).Append(",")
                .Append(CreateInternalSelectCountQuery(nameof(stats.EstatesCount), database.EstateRepository.Table))
                .Append(",")
                .Append(CreateInternalSelectCountQuery(nameof(stats.VehiclesCount), database.VehicleRepository.Table))
                .Append(",")
                .Append(CreateInternalSelectSumQuery(nameof(stats.HoursPlayedCount),
                    new[] {database.UserRepository.Table}, "hours")).Append(",")
                .Append(CreateInternalSelectSumQuery(nameof(stats.BankTotalMoney),
                    new[] {database.CharacterRepository.Table, database.FactionRepository.Table}, "bankmoney",
                    "bankbalance"))
                .Build();

            stats = await database.SelectQueryFirst<ServerStatsResult>(query);

            return stats;
        }

        #region private

        private static string CreateInternalSelectCountQuery(string propertyName, string table)
            => new SqlBuilder()
                .Open
                .SelectCount()
                .From(table)
                .Close
                .As(propertyName)
                .Build()
                .Query;

        private static string CreateInternalSelectSumQuery(string propertyName, string[] tables,
            params string[] columns)
            => new SqlBuilder()
                .Open
                .Select(false)
                .Sum(string.Join("+", columns))
                .From(string.Join(",", tables))
                .Close
                .As(propertyName)
                .Build()
                .Query;

        #endregion
    }
}