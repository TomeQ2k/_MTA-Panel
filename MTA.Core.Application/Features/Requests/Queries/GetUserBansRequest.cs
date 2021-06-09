using MediatR;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetUserBansRequest : IRequest<GetUserBansResponse>
    {
    }
}