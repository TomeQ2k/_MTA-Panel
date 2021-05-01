using System;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class AuthService : IAuthService
    {
        private readonly IDatabase database;
        private readonly IJwtAuthorizationTokenGenerator jwtAuthorizationTokenGenerator;
        private readonly IHashGenerator hashGenerator;

        public AuthService(IDatabase database, IJwtAuthorizationTokenGenerator jwtAuthorizationTokenGenerator,
            IHashGenerator hashGenerator)
        {
            this.database = database;
            this.jwtAuthorizationTokenGenerator = jwtAuthorizationTokenGenerator;
            this.hashGenerator = hashGenerator;
        }

        public async Task<SignInResult> SignIn(string login, string password)
        {
            User user;

            if (login.IsEmailAddress())
                user = await database.UserRepository.FindUserByEmail(login)
                       ?? throw new InvalidCredentialsException("Invalid login or password");
            else
                user = await database.UserRepository.FindUserByUsername(login)
                       ?? throw new InvalidCredentialsException("Invalid login or password");

            if (!user.IsActivated)
                throw new AccountNotConfirmedException("Account has been not activated");

            if (!hashGenerator.VerifyHash(password, user.PasswordSalt, user.PasswordHash))
                throw new InvalidCredentialsException("Invalid login or password");

            var token = jwtAuthorizationTokenGenerator.GenerateToken(user);

            return new SignInResult {JwtToken = token, User = user};
        }

        public async Task<SignUpResult> SignUp(string username, string email, string password, string serial,
            int referrerId)
        {
            var passwordSalt = hashGenerator.CreateSalt();
            var passwordHash = hashGenerator.GenerateHash(password, passwordSalt);

            var user = new UserBuilder()
                .SetUsername(username)
                .SetEmail(email)
                .SetPassword(passwordHash, passwordSalt)
                .SetSerial(serial)
                .SetReferrer(referrerId)
                .Build();

            using (var transaction = database.BeginTransaction().Transaction)
            {
                if (!await database.UserRepository.Insert(user))
                    throw new DatabaseException();

                user = await database.UserRepository.FindUserByUsername(username)
                       ?? throw new EntityNotFoundException("User not found");

                var serialToAdd = Serial.Create(user.Id, serial);
                var registerToken = Token.Create(TokenType.Register, user.Id);

                if (!await database.SerialRepository.Insert(serialToAdd))
                    throw new DatabaseException();

                if (!await database.TokenRepository.Insert(registerToken, false))
                    throw new DatabaseException();

                transaction.Complete();

                return new SignUpResult {User = user, TokenCode = registerToken.Code};
            }
        }

        public async Task<bool> ConfirmAccount(string email, string token)
        {
            var user = await database.UserRepository.GetUserByEmailWithTokenType(email, TokenType.Register) ??
                       throw new ServerException("Invalid token");

            if (user.Token.Code != token)
                throw new ServerException("Invalid token");

            if (user.Token.DateCreated < DateTime.Now.AddDays(-Constants.ConfirmAccountTokenExpireDays))
                throw new TokenExpiredException();

            user.ActivateAccount();

            await database.UserRepository.Update(user);

            return await database.TokenRepository.Delete(user.Token);
        }

        public async Task<SendActivationEmailResult> GenerateActivationEmailToken(string email)
        {
            var user = await database.UserRepository.FindUserByEmail(email);

            if (user == null || !user.IsActivated)
                return null;

            var token = await database.TokenRepository.GetTokenWithTypeByUserId(user.Id, TokenType.Register);

            if (token != null)
                await database.TokenRepository.Delete(token);

            var registerToken =
                Token.Create(TokenType.Register, user.Id);

            return await database.TokenRepository.Insert(registerToken, false)
                ? new SendActivationEmailResult
                    {Email = user.Email, Token = registerToken.Code, Username = user.Username}
                : null;
        }
    }
}