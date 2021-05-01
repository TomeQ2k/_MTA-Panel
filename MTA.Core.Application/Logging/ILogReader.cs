using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Logic.Requests.Queries.Params;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logging
{
    public interface ILogReader
    {
        Task<IEnumerable<MtaLogModel>> GetMtaLogsFromDatabase(MtaLogFiltersParams filters,
            IEnumerable<SourceAffectedModel> sourceAffectedModels);

        Task<PagedList<ApiLogModel>> GetApiLogsFromFile(ApiLogFiltersParams filters);
    }
}