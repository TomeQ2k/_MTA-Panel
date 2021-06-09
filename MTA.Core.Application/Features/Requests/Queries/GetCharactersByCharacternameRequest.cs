using MediatR;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetCharactersByCharacternameRequest : IRequest<GetCharactersByCharacternameResponse>
    {
        public string Charactername { get; init; }
    }
}