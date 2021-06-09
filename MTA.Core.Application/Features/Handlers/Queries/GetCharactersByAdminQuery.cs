using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetCharactersByAdminQuery : IRequestHandler<GetCharactersByAdminRequest,
        GetCharactersByAdminResponse>
    {
        private readonly IReadOnlyCharacterService characterService;
        private readonly IMapper mapper;
        private readonly IHttpContextWriter httpContextWriter;

        public GetCharactersByAdminQuery(IReadOnlyCharacterService characterService, IMapper mapper,
            IHttpContextWriter httpContextWriter)
        {
            this.characterService = characterService;
            this.mapper = mapper;
            this.httpContextWriter = httpContextWriter;
        }

        public async Task<GetCharactersByAdminResponse> Handle(GetCharactersByAdminRequest request,
            CancellationToken cancellationToken)
        {
            var characters = await characterService.GetCharactersByAdmin(request);

            httpContextWriter.AddPagination(characters.CurrentPage, characters.PageSize, characters.TotalCount,
                characters.TotalPages);

            return new GetCharactersByAdminResponse
                {Characters = mapper.Map<IEnumerable<CharacterAdminListDto>>(characters)};
        }
    }
}