using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logging
{
    public interface ILogReaderHelper
    {
        Task<IEnumerable<LogAction>> GetAllowedLogActions();
    }
}