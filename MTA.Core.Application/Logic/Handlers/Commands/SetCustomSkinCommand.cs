using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Handlers.Commands
{
    public class SetCustomSkinCommand : IRequestHandler<SetCustomSkinRequest, SetCustomSkinResponse>
    {
        public Task<SetCustomSkinResponse> Handle(SetCustomSkinRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}