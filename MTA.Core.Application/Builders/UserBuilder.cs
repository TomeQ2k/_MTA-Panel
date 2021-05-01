using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class UserBuilder : IUserBuilder
    {
        private readonly User user = new User();

        public IUserBuilder SetUsername(string username)
        {
            user.SetUsername(username);
            return this;
        }

        public IUserBuilder SetEmail(string email)
        {
            user.SetEmail(email);
            return this;
        }

        public IUserBuilder SetPassword(string passwordHash, string passwordSalt)
        {
            user.SetPassword(passwordHash, passwordSalt);
            return this;
        }

        public IUserBuilder SetSerial(string serial)
        {
            user.SetSerial(serial);
            return this;
        }

        public IUserBuilder SetReferrer(int referrerId)
        {
            user.SetReferrer(referrerId);
            return this;
        }

        public User Build() => user;
    }
}