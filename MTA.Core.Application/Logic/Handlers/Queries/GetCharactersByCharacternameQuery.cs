using System.Collections.Generic;
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
    public class GetCharactersByCharacternameQuery : IRequestHandler<GetCharactersByCharacternameRequest,
        GetCharactersByCharacternameResponse>
    {
        private readonly IReadOnlyCharacterService characterService;
        private readonly IMapper mapper;

        public GetCharactersByCharacternameQuery(IReadOnlyCharacterService characterService, IMapper mapper)
        {
            this.characterService = characterService;
            this.mapper = mapper;
        }

        public async Task<GetCharactersByCharacternameResponse> Handle(GetCharactersByCharacternameRequest request,
            CancellationToken cancellationToken)
            => new GetCharactersByCharacternameResponse
            {
                Characters =
                    mapper.Map<IEnumerable<CharacterListDto>>(await characterService.GetCharactersByCharactername(request.Charactername))
            };
    }
}