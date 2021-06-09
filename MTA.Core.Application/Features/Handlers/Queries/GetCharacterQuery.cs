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
    public class GetCharacterQuery : IRequestHandler<GetCharacterRequest, GetCharacterResponse>
    {
        private readonly IReadOnlyCharacterService characterService;
        private readonly IMapper mapper;

        public GetCharacterQuery(IReadOnlyCharacterService characterService, IMapper mapper)
        {
            this.characterService = characterService;
            this.mapper = mapper;
        }

        public async Task<GetCharacterResponse> Handle(GetCharacterRequest request,
            CancellationToken cancellationToken)
        {
            var character = await characterService.GetCharacter(request.CharacterId);

            return new GetCharacterResponse
                {Character = mapper.Map<CharacterDto>(character)};
        }
    }
}