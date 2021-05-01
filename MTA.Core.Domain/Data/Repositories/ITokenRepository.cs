using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface ITokenRepository : IRepository<Token>
    {
        Task<Token> GetTokenWithTypeByUserId(int userId, TokenType tokenType);

        Task<Token> GetTokenByCodeAndType(string code, TokenType tokenType, int userId);
    }
}