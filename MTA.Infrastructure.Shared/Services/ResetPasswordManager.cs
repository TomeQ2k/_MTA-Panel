using System;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ResetPasswordManager : IResetPasswordManager
    {
        private readonly IDatabase database;
        private readonly IHashGenerator hashGenerator;

        public ResetPasswordManager(IDatabase database, IHashGenerator hashGenerator)
        {
            this.database = database;
            this.hashGenerator = hashGenerator;
        }

        public async Task<bool> ResetPassword(string email, string token, string newPassword)
        {
            var user = await GetAndVerifyUserByEmail(email, token);

            var passwordSalt = hashGenerator.CreateSalt();
            var passwordHash = hashGenerator.GenerateHash(newPassword, passwordSalt);

            user.SetPassword(passwordHash, passwordSalt);

            if (await database.UserRepository.Update(user))
                return await database.TokenRepository.Delete(user.Token);

            throw new DatabaseException();
        }

        public async Task<bool> VerifyResetPasswordToken(string email, string token)
            => await GetAndVerifyUserByEmail(email, token) != null;

        #region private

        private async Task<User> GetAndVerifyUserByEmail(string email, string token)
        {
            var user = await database.UserRepository.GetUserByEmailWithTokenType(email, TokenType.ResetPassword) ??
                       throw new ServerException("Invalid token");

            if (user.Token.Code != token)
                throw new ServerException("Invalid token");

            if (user.Token.DateCreated < DateTime.Now.AddDays(-Constants.ResetPasswordTokenExpireDays))
                throw new TokenExpiredException();

            return user;
        }

        #endregion
    }
}