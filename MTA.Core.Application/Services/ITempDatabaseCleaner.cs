using System.Threading.Tasks;

namespace MTA.Core.Application.Services
{
    public interface ITempDatabaseCleaner
    {
        Task ClearGameTempObjectsAndInteriors();
    }
}