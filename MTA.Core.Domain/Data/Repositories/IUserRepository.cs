using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> FindUserById(int userId);
        Task<User> FindUserByEmail(string email);
        Task<User> FindUserByUsername(string username);
        Task<User> FindUserWithSerials(int userId);
        Task<User> GetUserByEmailWithTokenType(string email, TokenType tokenType);
        Task<User> GetUserWithCharacters(int userId);

        Task<IEnumerable<User>> GetUsersByUsername(string username);
        Task<IEnumerable<User>> GetUsersByAdmin(IAdminUserFiltersParams request);

        Task<IEnumerable<User>> GetUsersWithAssignedReports(ReportCategoryType reportCategoryType,
            bool isPrivate = false);

        Task<IEnumerable<User>> GetMostActiveAdmins(int top = Constants.TopStatsLimit);

        Task<bool> SerialExists(string serial);
    }
}