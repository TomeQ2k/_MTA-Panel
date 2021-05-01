using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class BanService : IBanService
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public BanService(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<IEnumerable<Ban>> GetUserBans()
            => await database.BanRepository.GetWhere(new SqlBuilder()
                .Append("account").Equals
                .Append(httpContextReader.CurrentUserId)
                .And.Append("admin").NotEquals.Append(0)
                .Build().Query);

        public async Task<Ban> AddBan(Ban ban)
            => await database.BanRepository.Insert(ban) ? ban : null;
    }
}