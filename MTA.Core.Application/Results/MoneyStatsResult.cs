using System.Collections.Generic;
using MTA.Core.Application.Dtos;

namespace MTA.Core.Application.Results
{
    public record MoneyStatsResult
    {
        public IEnumerable<TopCharacterByMoneyDto> TopCharactersByMoney { get; init; }
    }
}