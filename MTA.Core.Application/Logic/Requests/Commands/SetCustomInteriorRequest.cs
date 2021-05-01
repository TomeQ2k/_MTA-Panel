using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record SetCustomInteriorRequest : IRequest<SetCustomInteriorResponse>
    {
    }
}