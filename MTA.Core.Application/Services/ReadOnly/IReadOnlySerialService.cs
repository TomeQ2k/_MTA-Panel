using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlySerialService
    {
        Task<IEnumerable<Serial>> GetUserSerials(int userId);

        Task<bool> SerialExists(string serial, int userId);
        Task<bool> SerialExists(string serial);
    }
}