using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface ISerialRepository : IRepository<Serial>
    {
        Task<IEnumerable<Serial>> GetUserSerials(int userId);

        Task<bool> DeleteSerial(int serialId, int userId);

        Task<bool> SerialExists(string serial, int userId);
    }
}