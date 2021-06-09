using System.Collections.Generic;
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
    public class GetUserSerialsQuery : IRequestHandler<GetUserSerialsRequest, GetUserSerialsResponse>
    {
        private readonly IReadOnlySerialService serialService;
        private readonly IMapper mapper;

        public GetUserSerialsQuery(IReadOnlySerialService serialService, IMapper mapper)
        {
            this.serialService = serialService;
            this.mapper = mapper;
        }

        public async Task<GetUserSerialsResponse> Handle(GetUserSerialsRequest request,
            CancellationToken cancellationToken)
            => new GetUserSerialsResponse
                {UserSerials = mapper.Map<IEnumerable<SerialDto>>(await serialService.GetUserSerials(request.UserId))};
    }
}