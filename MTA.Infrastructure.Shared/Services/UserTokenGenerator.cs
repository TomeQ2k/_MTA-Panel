using System.Threading.Tasks;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class UserTokenGenerator : IUserTokenGenerator
    {
        private readonly IDatabase database;
        private readonly IReadOnlyAccountManager accountManager;
        private readonly IHashGenerator hashGenerator;

        public UserTokenGenerator(IDatabase database, IReadOnlyAccountManager accountManager,
            IHashGenerator hashGenerator)
        {
            this.database = database;
            this.accountManager = accountManager;
            this.hashGenerator = hashGenerator;
        }

        public async Task<GenerateResetPasswordTokenResult> GenerateResetPasswordToken(string login)
        {
            var user = login.IsEmailAddress()
                ? await database.UserRepository.FindUserByEmail(login)
                : await database.UserRepository.FindUserByUsername(login);

            if (user == null)
                return null;

            var token = await database.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.ResetPassword);

            if (token != null)
                await database.TokenRepository.Delete(token);

            var resetPasswordToken = Token.Create(TokenType.ResetPassword, user.Id);

            return await database.TokenRepository.Insert(resetPasswordToken, false)
                ? new GenerateResetPasswordTokenResult
                    {Email = user.Email, Username = user.Username, Token = resetPasswordToken.Code}
                : throw new DatabaseException();
        }

        public async Task<GenerateChangePasswordTokenResult> GenerateChangePasswordToken(string oldPassword)
        {
            var user = await accountManager.GetCurrentUser() ?? throw new EntityNotFoundException("User not found");

            if (!hashGenerator.VerifyHash(oldPassword, user.PasswordSalt, user.PasswordHash))
                throw new OldPasswordInvalidException("Incorrect old password");

            var token = await CreateToken(user.Id, TokenType.ChangePassword);

            return await database.TokenRepository.Insert(token, false)
                ? new GenerateChangePasswordTokenResult
                    {Email = user.Email, Token = token.Code, Username = user.Username}
                : throw new DatabaseException();
        }

        public async Task<GenerateChangeEmailTokenResult> GenerateChangeEmailToken()
        {
            var user = await accountManager.GetCurrentUser() ?? throw new EntityNotFoundException("User not found");

            var token = await CreateToken(user.Id, TokenType.ChangeEmail);

            return await database.TokenRepository.Insert(token, false)
                ? new GenerateChangeEmailTokenResult
                    {Email = user.Email, Token = token.Code, Username = user.Username}
                : throw new DatabaseException();
        }

        public async Task<GenerateChangeEmailTokenResult> GenerateChangeEmailTokenByAdmin(int userId, string newEmail)
        {
            var user = await database.UserRepository.FindById(userId) ??
                       throw new EntityNotFoundException("User not found");

            var token = await CreateToken(user.Id, TokenType.ChangeEmail);

            return await database.TokenRepository.Insert(token, false)
                ? new GenerateChangeEmailTokenResult
                    {Email = newEmail, Token = token.Code, Username = user.Username}
                : throw new DatabaseException();
        }

        public async Task<GenerateAddSerialTokenResult> GenerateAddSerialToken()
        {
            var user = await accountManager.GetCurrentUser() ?? throw new EntityNotFoundException("User not found");

            var token = await CreateToken(user.Id, TokenType.AddSerial);

            return await database.TokenRepository.Insert(token, false)
                ? new GenerateAddSerialTokenResult
                    {Email = user.Email, Token = token.Code, Username = user.Username}
                : throw new DatabaseException();
        }

        #region private

        private async Task<Token> CreateToken(int userId, TokenType tokenType)
        {
            var token = await database.TokenRepository.GetTokenWithTypeByUserId(userId, tokenType);

            if (token != null)
                await database.TokenRepository.Delete(token);

            return Token.Create(tokenType, userId);
        }

        #endregion
    }
}