using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Data.Repositories.Params;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IPhoneSmsRepository : IRepository<PhoneSms>
    {
        Task<IEnumerable<PhoneSms>> GetPhoneSms(IMtaLogFiltersParams filters, IList<string> sourceAffectedValues);
    }
}