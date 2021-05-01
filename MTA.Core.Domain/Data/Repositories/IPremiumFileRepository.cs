using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IPremiumFileRepository : IRepository<PremiumFile>
    {
        Task<PremiumFile> GetFileWithOrder(string premiumFileId);
        Task<PremiumFile> GetFileWithOrderAndEstate(string premiumFileId);
    }
}