using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Results;
using MTA.Core.Domain.Data;

namespace MTA.Infrastructure.Shared.Services
{
    public class AdminsActivityStatsService : BaseStatsService<AdminsActivityStatsResult>
    {
        private readonly IMapper mapper;

        public AdminsActivityStatsService(IDatabase database, IMapper mapper) : base(database)
        {
            this.mapper = mapper;
        }

        public async override Task<AdminsActivityStatsResult> SelectStats()
            => new AdminsActivityStatsResult
            {
                MostActiveAdmins =
                    mapper.Map<IEnumerable<MostActiveAdminUserDto>>(await database.UserRepository.GetMostActiveAdmins())
            };
    }
}