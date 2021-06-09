using MediatR;
using MTA.Core.Application.Features.Requests.Queries.Params;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetCharactersByAdminRequest : AdminCharacterFiltersParams,
        IRequest<GetCharactersByAdminResponse>
    {
    }
}