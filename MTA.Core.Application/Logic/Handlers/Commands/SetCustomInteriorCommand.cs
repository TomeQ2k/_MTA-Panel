using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class SetCustomInteriorCommand : IRequestHandler<SetCustomInteriorRequest, SetCustomInteriorResponse>
    {
        public Task<SetCustomInteriorResponse> Handle(SetCustomInteriorRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}