using System.Threading.Tasks;

namespace MTA.Core.Application.Services
{
    public interface ITokenCleaner
    {
        Task ClearTokens();
    }
}