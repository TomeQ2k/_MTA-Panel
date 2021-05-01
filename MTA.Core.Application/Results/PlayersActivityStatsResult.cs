using System.Collections.Generic;
using MTA.Core.Application.Dtos;

namespace MTA.Core.Application.Results
{
    public record PlayersActivityStatsResult
    {
        public IEnumerable<MostActiveCharacterDto> MostActiveCharacters { get; init; }
    }
}