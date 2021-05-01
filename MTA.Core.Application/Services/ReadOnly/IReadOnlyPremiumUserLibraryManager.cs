using System.Threading.Tasks;
using MTA.Core.Application.Results;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyPremiumUserLibraryManager
    {
        Task<PremiumUserLibraryResult> FetchLibraryFiles();
    }
}