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
    public class GetCurrentUserQuery : IRequestHandler<GetCurrentUserRequest, GetCurrentUserResponse>
    {
        private readonly IReadOnlyAccountManager accountManager;
        private readonly IMapper mapper;

        public GetCurrentUserQuery(IReadOnlyAccountManager accountManager, IMapper mapper)
        {
            this.accountManager = accountManager;
            this.mapper = mapper;
        }

        public async Task<GetCurrentUserResponse> Handle(GetCurrentUserRequest request,
            CancellationToken cancellationToken)
        {
            var user = await accountManager.GetCurrentUser();

            return new GetCurrentUserResponse {User = mapper.Map<UserWithCharactersDto>(user)};
        }
    }
}