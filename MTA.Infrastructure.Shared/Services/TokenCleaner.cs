using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class TokenCleaner : ITokenCleaner
    {
        private readonly IDatabase database;

        public TokenCleaner(IDatabase database)
        {
            this.database = database;
        }

        public Task ClearTokens()
            => Task.FromResult(database.TokenRepository.Execute(new SqlBuilder()
                .Delete(database.TokenRepository.Table)
                .Where("dateCreated")
                .Lesser
                .Append($"DATE_ADD(NOW(), INTERVAL -{Constants.ConfirmAccountTokenExpireDays / 60 / 24} DAY)")
                .Build()));
    }
}