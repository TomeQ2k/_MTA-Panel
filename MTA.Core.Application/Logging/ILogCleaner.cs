using System.Threading.Tasks;

namespace MTA.Core.Application.Logging
{
    public interface ILogCleaner
    {
        Task<bool> ClearLogs();
    }
}