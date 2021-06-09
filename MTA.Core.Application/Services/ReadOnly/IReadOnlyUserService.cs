using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Models;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyUserService
    {
        Task<User> GetUserWithSerials(int userId);
        Task<User> GetUserWithCharacters(int userId);
        Task<User> FindUserByUsername(string username);

        Task<IEnumerable<User>> GetUsersByUsername(string username);
        Task<PagedList<User>> GetUsersByAdmin(GetUsersByAdminRequest request);
    }
}