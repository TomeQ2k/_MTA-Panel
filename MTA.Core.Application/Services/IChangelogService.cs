using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IChangelogService : IReadOnlyChangelogService
    {
        Task<Changelog> CreateChangelog(CreateChangelogRequest request);

        Task<Changelog> UpdateChangelog(UpdateChangelogRequest request);

        Task<bool> DeleteChangelog(string changelogId);
    }
}