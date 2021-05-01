using System.Threading.Tasks;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services
{
    public interface IUserTokenGenerator
    {
        Task<GenerateResetPasswordTokenResult> GenerateResetPasswordToken(string login);
        Task<GenerateChangePasswordTokenResult> GenerateChangePasswordToken(string oldPassword);
        Task<GenerateChangeEmailTokenResult> GenerateChangeEmailToken();
        Task<GenerateAddSerialTokenResult> GenerateAddSerialToken();

        Task<GenerateChangeEmailTokenResult> GenerateChangeEmailTokenByAdmin(int userId, string newEmail);
    }
}