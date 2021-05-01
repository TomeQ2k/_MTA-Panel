using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyRPTestManager
    {
        Task<IEnumerable<Question>> GetPartQuestions(RPTestPartType partType);
    }
}