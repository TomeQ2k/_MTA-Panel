using System.Collections.Generic;
using MTA.Core.Application.Dtos;

namespace MTA.Core.Application.Results
{
    public record FactionsStatsResult
    {
        public IEnumerable<TopFactionByBankBalanceDto> TopFactionsByBankBalance { get; init; }
    }
}