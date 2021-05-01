using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        public ApplicationRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<Application> GetApplicationWithApplicant(int applicationId)
            => (await QueryJoin<Application, User>(new SqlBuilder()
                    .Select()
                    .From(Table).As("a")
                    .Join("a.applicant", new(RepositoryDictionary.FindTable(typeof(IUserRepository)), "id"), "k")
                    .Build(), (application, applicant) =>
                {
                    application.SetApplicant(applicant);
                    return application;
                }))
                .FirstOrDefault();
    }
}