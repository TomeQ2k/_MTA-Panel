using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Services
{
    public interface IAccountManager : IReadOnlyAccountManager
    {
        Task<bool> ChangePassword(string newPassword, string email, string token);
        Task<bool> ChangeEmail(string newEmail, string email, string token);
        
        Task<bool> AddSerial(string serial, string email, string token);
    }
}