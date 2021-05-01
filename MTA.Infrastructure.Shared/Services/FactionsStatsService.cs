using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Results;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class FactionsStatsService : BaseStatsService<FactionsStatsResult>
    {
        private readonly IMapper mapper;

        public FactionsStatsService(IDatabase database, IMapper mapper) : base(database)
        {
            this.mapper = mapper;
        }

        public async override Task<FactionsStatsResult> SelectStats()
            => new FactionsStatsResult
            {
                TopFactionsByBankBalance =
                    mapper.Map<IEnumerable<TopFactionByBankBalanceDto>>(await database.FactionRepository
                        .GetTopFactionsByBankBalance())
            };
    }
}