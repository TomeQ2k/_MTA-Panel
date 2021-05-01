using System.Threading.Tasks;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IApplicationRepository : IRepository<Entities.Application>
    {
        Task<Entities.Application> GetApplicationWithApplicant(int applicationId);
    }
}