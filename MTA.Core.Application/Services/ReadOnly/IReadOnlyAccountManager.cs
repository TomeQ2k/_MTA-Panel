using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyAccountManager
    {
        Task<User> GetCurrentUser();
    }
}