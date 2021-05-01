using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetAdminActionsByActionAndUserIdQuery
        : IRequestHandler<GetAdminActionsByActionAndUserIdRequest, GetAdminActionsByActionAndUserIdResponse>
    {
        private readonly IReadOnlyAdminActionService adminActionService;
        private readonly IMapper mapper;

        public GetAdminActionsByActionAndUserIdQuery(IReadOnlyAdminActionService adminActionService, IMapper mapper)
        {
            this.adminActionService = adminActionService;
            this.mapper = mapper;
        }

        public async Task<GetAdminActionsByActionAndUserIdResponse> Handle(GetAdminActionsByActionAndUserIdRequest andUserIdRequest,
            CancellationToken cancellationToken)
            => new GetAdminActionsByActionAndUserIdResponse
            {
                AdminActions =
                    mapper.Map<IEnumerable<AdminActionDto>>(
                        await adminActionService.GetAdminActionsByActionAndUserId(andUserIdRequest.ActionType))
            };
    }
}