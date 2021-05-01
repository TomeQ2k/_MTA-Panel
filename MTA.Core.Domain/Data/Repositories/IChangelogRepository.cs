using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IChangelogRepository : IRepository<Changelog>
    {
        Task<IEnumerable<Changelog>> GetChangelogsWithDetails();
    }
}