using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record ClearAllNotificationsRequest : IRequest<ClearAllNotificationsResponse>
    {
    }
}