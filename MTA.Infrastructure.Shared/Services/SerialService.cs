using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class SerialService : ISerialService
    {
        private readonly IDatabase database;
        private readonly IHttpContextReader httpContextReader;

        public SerialService(IDatabase database, IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.httpContextReader = httpContextReader;
        }

        public async Task<IEnumerable<Serial>> GetUserSerials(int userId)
            => await database.SerialRepository.GetUserSerials(userId);

        public async Task<bool> DeleteSerial(int serialId)
            => await database.SerialRepository.DeleteSerial(serialId, httpContextReader.CurrentUserId);

        public async Task<bool> SerialExists(string serial, int userId)
            => await database.SerialRepository.SerialExists(serial, userId);

        public async Task<bool> SerialExists(string serial)
            => await database.UserRepository.SerialExists(serial);
    }
}