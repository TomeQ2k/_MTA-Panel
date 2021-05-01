using System.Threading.Tasks;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services
{
    public interface IAuthService
    {
        Task<SignInResult> SignIn(string login, string password);
        Task<SignUpResult> SignUp(string username, string email, string password, string serial, int referrerId);

        Task<bool> ConfirmAccount(string email, string token);
        Task<SendActivationEmailResult> GenerateActivationEmailToken(string email);
    }
}