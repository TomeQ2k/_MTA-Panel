using System.Threading.Tasks;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class AuthValidationService : IAuthValidationService
    {
        private readonly IDatabase database;

        public AuthValidationService(IDatabase database)
        {
            this.database = database;
        }

        public async Task<bool> UsernameExists(string username)
            => await database.UserRepository.FindUserByUsername(username) != null;

        public async Task<bool> EmailExists(string email)
            => await database.UserRepository.FindUserByEmail(email) != null;
    }
}