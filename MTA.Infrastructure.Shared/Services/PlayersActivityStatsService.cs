using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Results;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class PlayersActivityStatsService : BaseStatsService<PlayersActivityStatsResult>
    {
        private readonly IMapper mapper;

        public PlayersActivityStatsService(IDatabase database, IMapper mapper) : base(database)
        {
            this.mapper = mapper;
        }

        public async override Task<PlayersActivityStatsResult> SelectStats()
            => new PlayersActivityStatsResult
            {
                MostActiveCharacters =
                    mapper.Map<IEnumerable<MostActiveCharacterDto>>(await database.CharacterRepository
                        .GetMostActiveCharacters())
            };
    }
}