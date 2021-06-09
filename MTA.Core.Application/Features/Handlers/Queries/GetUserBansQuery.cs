using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetUserBansQuery : IRequestHandler<GetUserBansRequest, GetUserBansResponse>
    {
        private readonly IReadOnlyBanService banService;
        private readonly IReadOnlyAdminActionService adminActionService;
        private readonly IMapper mapper;

        public GetUserBansQuery(IReadOnlyBanService banService, IReadOnlyAdminActionService adminActionService,
            IMapper mapper)
        {
            this.banService = banService;
            this.adminActionService = adminActionService;
            this.mapper = mapper;
        }

        public async Task<GetUserBansResponse> Handle(GetUserBansRequest request, CancellationToken cancellationToken)
        {
            var penalties = await adminActionService.GetAdminActionsAsUserBans();

            if (penalties.Any())
                return new GetUserBansResponse {Bans = mapper.Map<IEnumerable<PenaltyDto>>(penalties)};

            var bans = await banService.GetUserBans();

            return new GetUserBansResponse {Bans = mapper.Map<IEnumerable<PenaltyDto>>(bans)};
        }
    }
}