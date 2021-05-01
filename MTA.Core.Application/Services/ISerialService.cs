using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Services
{
    public interface ISerialService : IReadOnlySerialService
    {
        Task<bool> DeleteSerial(int serialId);
    }
}