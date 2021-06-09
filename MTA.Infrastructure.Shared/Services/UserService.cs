using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class UserService : IUserService
    {
        private readonly IDatabase database;

        public UserService(IDatabase database)
        {
            this.database = database;
        }

        public async Task<User> GetUserWithSerials(int userId)
            => await database.UserRepository.FindUserWithSerials(userId);

        public async Task<User> GetUserWithCharacters(int userId)
            => (await database.UserRepository.GetUserWithCharacters(userId)).SetBanId(
                   (await database.BanRepository.FindByColumn(new("account", userId)))?.Id) ??
               throw new EntityNotFoundException("User not found");

        public async Task<User> FindUserByUsername(string username)
            => await database.UserRepository.FindUserByUsername(username);

        public async Task<IEnumerable<User>> GetUsersByUsername(string username)
            => await database.UserRepository.GetUsersByUsername(username);

        public async Task<PagedList<User>> GetUsersByAdmin(GetUsersByAdminRequest request)
            => (await database.UserRepository.GetUsersByAdmin(request)).ToPagedList(request.PageNumber,
                request.PageSize);
    }
}