using System.Collections.Generic;
using MTA.Core.Application.Dtos;

namespace MTA.Core.Application.Results
{
    public record AdminsActivityStatsResult
    {
        public IEnumerable<MostActiveAdminUserDto> MostActiveAdmins { get; init; }
    }
}