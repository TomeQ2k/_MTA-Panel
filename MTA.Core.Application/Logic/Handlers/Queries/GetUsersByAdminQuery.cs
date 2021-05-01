using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class
        GetUsersByAdminQuery : IRequestHandler<GetUsersByAdminRequest, GetUsersByAdminResponse>
    {
        private readonly IReadOnlyUserService userService;
        private readonly IMapper mapper;
        private readonly IHttpContextWriter httpContextWriter;

        public GetUsersByAdminQuery(IReadOnlyUserService userService, IMapper mapper,
            IHttpContextWriter httpContextWriter)
        {
            this.userService = userService;
            this.mapper = mapper;
            this.httpContextWriter = httpContextWriter;
        }

        public async Task<GetUsersByAdminResponse> Handle(GetUsersByAdminRequest request,
            CancellationToken cancellationToken)
        {
            var users = await userService.GetUsersByAdmin(request);

            httpContextWriter.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return new GetUsersByAdminResponse {Users = mapper.Map<IEnumerable<UserAdminListDto>>(users)};
        }
    }
}