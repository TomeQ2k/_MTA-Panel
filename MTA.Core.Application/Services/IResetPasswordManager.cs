using System.Threading.Tasks;

namespace MTA.Core.Application.Services
{
    public interface IResetPasswordManager
    {
        Task<bool> ResetPassword(string email, string token, string newPassword);
        Task<bool> VerifyResetPasswordToken(string email, string token);
    }
}