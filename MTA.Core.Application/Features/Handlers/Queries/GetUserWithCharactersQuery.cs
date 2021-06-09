using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetUserWithCharactersQuery : IRequestHandler<GetUserWithCharactersRequest,
        GetUserWithCharactersResponse>
    {
        private readonly IReadOnlyUserService userService;
        private readonly IMapper mapper;

        public GetUserWithCharactersQuery(IReadOnlyUserService userService, IMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }


        public async Task<GetUserWithCharactersResponse> Handle(GetUserWithCharactersRequest request,
            CancellationToken cancellationToken)
        {
            var user = await userService.GetUserWithCharacters(request.UserId)
                       ?? throw new EntityNotFoundException("User not found");

            return new GetUserWithCharactersResponse {User = mapper.Map<UserWithCharactersDto>(user)};
        }
    }
}