using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IUserBuilder : IBuilder<User>
    {
        IUserBuilder SetUsername(string username);
        IUserBuilder SetEmail(string email);
        IUserBuilder SetPassword(string passwordHash, string passwordSalt);
        IUserBuilder SetSerial(string serial);
        IUserBuilder SetReferrer(int referrerId);
    }
}