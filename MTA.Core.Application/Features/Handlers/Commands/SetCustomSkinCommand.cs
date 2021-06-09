using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class SetCustomSkinCommand : IRequestHandler<SetCustomSkinRequest, SetCustomSkinResponse>
    {
        public Task<SetCustomSkinResponse> Handle(SetCustomSkinRequest request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}