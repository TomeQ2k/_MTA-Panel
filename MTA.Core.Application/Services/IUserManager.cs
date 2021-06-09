using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services
{
    public interface IUserManager
    {
        Task<bool> BlockAccount(BlockAccountRequest request);
        Task<AddCreditsResult> AddCredits(int credits, int userId);
        Task<bool> CleanAccount(int userId);
    }
}