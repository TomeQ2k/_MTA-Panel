using System.Threading.Tasks;

namespace MTA.Core.Application.Services
{
    public interface IAuthValidationService
    {
        Task<bool> UsernameExists(string username);
        Task<bool> EmailExists(string email);
    }
}