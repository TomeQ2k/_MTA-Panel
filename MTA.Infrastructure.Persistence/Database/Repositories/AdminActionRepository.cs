using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class AdminActionRepository : Repository<AdminAction>, IAdminActionRepository
    {
        public AdminActionRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<IEnumerable<AdminAction>> GetAdminActionsWithUserAndCharacterNamesByActionAndUserId(
            AdminActionType actionType, int userId)
            => await Query(new SqlBuilder()
                .Select("ah.id", "ah.date", "ah.action", "ah.duration", "ah.reason")
                .Append(",").Append("ku.username").As("UserName")
                .Append(",").Append("ka.username").As("AdminName")
                .Append(",").Append("p.charactername").As("CharacterName")
                .From(Table).As("ah")
                .LeftJoin("ah.user", new(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "ku")
                .LeftJoin("ah.admin", new(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "ka")
                .LeftJoin("ah.user_char", new(RepositoryDictionary.FindTable(typeof(ICharacterRepository)), "id"), "p")
                .Where("ah.action").Equals.Append((int) actionType)
                .And.Append("ah.user").Equals.Append(userId)
                .OrderBy("ah.date", OrderByType.Descending)
                .Build());

        public async Task<IEnumerable<AdminAction>> GetAdminActionsAsUserBans(int userId)
            => await Query(new SqlBuilder()
                .Select("ah.id", "ah.date", "ah.reason", "ah.duration")
                .Append(",").Append("ah.user").As("AccountId")
                .Append(",").Append("ah.admin").As("AdminId")
                .From(Table).As("ah")
                .Where("ah.user").Equals.Append(userId)
                .And.Append("ah.action").Equals.Append((int) AdminActionType.Ban)
                .Build());
    }
}