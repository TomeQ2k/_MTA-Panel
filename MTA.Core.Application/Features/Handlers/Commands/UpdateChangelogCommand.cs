using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class UpdateChangelogCommand : IRequestHandler<UpdateChangelogRequest, UpdateChangelogResponse>
    {
        private readonly IChangelogService changelogService;
        private readonly IMapper mapper;

        public UpdateChangelogCommand(IChangelogService changelogService, IMapper mapper)
        {
            this.changelogService = changelogService;
            this.mapper = mapper;
        }

        public async Task<UpdateChangelogResponse> Handle(UpdateChangelogRequest request,
            CancellationToken cancellationToken)
        {
            var changelog = await changelogService.UpdateChangelog(request) ??
                            throw new CrudException("Updating changelog failed");

            return new UpdateChangelogResponse {Changelog = mapper.Map<ChangelogDto>(changelog)};
        }
    }
}