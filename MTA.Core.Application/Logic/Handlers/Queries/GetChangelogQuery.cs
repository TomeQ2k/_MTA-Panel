using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Logic.Requests.Queries;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Queries
{
    public class GetChangelogQuery : IRequestHandler<GetChangelogsRequest, GetChangelogsResponse>
    {
        private readonly IChangelogService changelogService;
        private readonly IMapper mapper;

        public GetChangelogQuery(IChangelogService changelogService, IMapper mapper)
        {
            this.changelogService = changelogService;
            this.mapper = mapper;
        }

        public async Task<GetChangelogsResponse> Handle(GetChangelogsRequest request,
            CancellationToken cancellationToken)
        {
            var changelogs = await changelogService.GetChangelogs();

            return new GetChangelogsResponse {Changelogs = mapper.Map<IEnumerable<ChangelogDto>>(changelogs)};
        }
    }
}