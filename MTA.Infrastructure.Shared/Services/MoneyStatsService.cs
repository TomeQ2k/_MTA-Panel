using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Results;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class MoneyStatsService : BaseStatsService<MoneyStatsResult>
    {
        private readonly IMapper mapper;

        public MoneyStatsService(IDatabase database, IMapper mapper) : base(database)
        {
            this.mapper = mapper;
        }

        public async override Task<MoneyStatsResult> SelectStats()
            => new MoneyStatsResult
            {
                TopCharactersByMoney =
                    mapper.Map<IEnumerable<TopCharacterByMoneyDto>>(await database.CharacterRepository
                        .GetRichestCharacters())
            };
    }
}