using System;
using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class AccountManager : IAccountManager
    {
        private readonly IDatabase database;
        private readonly IHashGenerator hashGenerator;
        private readonly IReadOnlyUserService userService;
        private readonly IHttpContextReader httpContextReader;

        public AccountManager(IDatabase database, IHashGenerator hashGenerator, IReadOnlyUserService userService,
            IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.hashGenerator = hashGenerator;
            this.userService = userService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<User> GetCurrentUser()
            => await userService.GetUserWithCharacters(httpContextReader.CurrentUserId);

        public async Task<bool> ChangePassword(string newPassword, string email, string token)
        {
            var user = await GetUserWithToken(email, TokenType.ChangePassword, token);

            var passwordSalt = hashGenerator.CreateSalt();
            var passwordHash = hashGenerator.GenerateHash(newPassword, passwordSalt);

            user.SetPassword(passwordHash, passwordSalt);

            if (await database.UserRepository.Update(user))
                return await database.TokenRepository.Delete(user.Token);

            throw new DatabaseException();
        }

        public async Task<bool> ChangeEmail(string newEmail, string email, string token)
        {
            var user = await GetUserWithToken(email, TokenType.ChangeEmail, token);

            user.SetEmail(newEmail);

            if (await database.UserRepository.Update(user))
                return await database.TokenRepository.Delete(user.Token);

            throw new DatabaseException();
        }

        public async Task<bool> AddSerial(string serial, string email, string token)
        {
            var user = await GetUserWithToken(email, TokenType.AddSerial, token);

            var serialToAdd = Serial.Create(user.Id, serial);

            if (await database.SerialRepository.Insert(serialToAdd))
                return await database.TokenRepository.Delete(user.Token);

            throw new DatabaseException();
        }

        #region private

        private async Task<User> GetUserWithToken(string email, TokenType tokenType, string token)
        {
            var user = await database.UserRepository.GetUserByEmailWithTokenType(email, tokenType) ??
                       throw new ServerException("Invalid token");

            if (user.Token.Code != token)
                throw new ServerException("Invalid token");

            if (user.Token.DateCreated < DateTime.Now.AddDays(-Constants.ChangePasswordTokenExpireDays))
                throw new TokenExpiredException();

            return user;
        }

        #endregion
    }
}