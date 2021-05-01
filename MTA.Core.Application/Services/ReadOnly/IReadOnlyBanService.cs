using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyBanService
    {
        Task<IEnumerable<Ban>> GetUserBans();
    }
}