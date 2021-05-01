using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IAuthorizationTokenGenerator
    {
        string GenerateToken(User user);
    }
}