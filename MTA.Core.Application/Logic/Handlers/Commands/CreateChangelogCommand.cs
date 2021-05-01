using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class CreateChangelogCommand : IRequestHandler<CreateChangelogRequest, CreateChangelogResponse>
    {
        private readonly IChangelogService changelogService;
        private readonly IMapper mapper;

        public CreateChangelogCommand(IChangelogService changelogService, IMapper mapper)
        {
            this.changelogService = changelogService;
            this.mapper = mapper;
        }

        public async Task<CreateChangelogResponse> Handle(CreateChangelogRequest request,
            CancellationToken cancellationToken)
        {
            var changelog = await changelogService.CreateChangelog(request) ??
                            throw new CrudException("Creating changelog failed");

            return new CreateChangelogResponse {Changelog = mapper.Map<ChangelogDto>(changelog)};
        }
    }
}