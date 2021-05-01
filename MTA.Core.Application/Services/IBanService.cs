using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IBanService : IReadOnlyBanService
    {
        Task<Ban> AddBan(Ban ban);
    }
}