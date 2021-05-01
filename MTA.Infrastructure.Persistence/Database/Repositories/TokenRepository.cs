using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class TokenRepository : Repository<Token>, ITokenRepository

    {
        public TokenRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<Token> GetTokenWithTypeByUserId(int userId, TokenType tokenType)
            => await QueryFirst(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("userId")
                .Equals
                .Append(userId)
                .And
                .Append("tokenType")
                .Equals
                .Append((int) tokenType)
                .Build());

        public async Task<Token> GetTokenByCodeAndType(string code, TokenType tokenType, int userId)
            => await QueryFirst(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("code")
                .Equals.Append($"'{code}'")
                .And.Append("tokenType")
                .Equals.Append((int) tokenType)
                .And.Append("userId").Equals.Append(userId)
                .Build());
    }
}