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
    public class GetCharactersWithUserByCharacternameQuery :
        IRequestHandler<GetCharactersWithUserByCharacternameRequest, GetCharactersWithUserByCharacternameResponse>
    {
        private readonly IReadOnlyCharacterService characterService;
        private readonly IMapper mapper;

        public GetCharactersWithUserByCharacternameQuery(IReadOnlyCharacterService characterService, IMapper mapper)
        {
            this.characterService = characterService;
            this.mapper = mapper;
        }

        public async Task<GetCharactersWithUserByCharacternameResponse> Handle(
            GetCharactersWithUserByCharacternameRequest request,
            CancellationToken cancellationToken)
            => new GetCharactersWithUserByCharacternameResponse
            {
                CharactersWithUser =
                    mapper.Map<IEnumerable<CharacterWithUserDto>>(
                        await characterService.GetCharactersWithUserByCharactername(request.Charactername)),
            };
    }
}