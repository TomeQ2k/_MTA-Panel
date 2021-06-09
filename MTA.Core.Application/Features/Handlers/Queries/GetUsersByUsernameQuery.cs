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
    public class GetUsersByUsernameQuery : IRequestHandler<GetUsersByUsernameRequest, GetUsersByUsernameResponse>
    {
        private readonly IReadOnlyUserService userService;
        private readonly IMapper mapper;

        public GetUsersByUsernameQuery(IReadOnlyUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        public async Task<GetUsersByUsernameResponse> Handle(GetUsersByUsernameRequest request,
            CancellationToken cancellationToken)
            => new GetUsersByUsernameResponse
                {Users = mapper.Map<IEnumerable<UserListDto>>(await userService.GetUsersByUsername(request.Username))};
    }
}