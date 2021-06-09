using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class PassRPTestPartOneCommand : IRequestHandler<PassRPTestPartOneRequest, PassRPTestPartOneResponse>
    {
        private readonly IRPTestManager rpTestManager;

        public PassRPTestPartOneCommand(IRPTestManager rpTestManager)
        {
            this.rpTestManager = rpTestManager;
        }

        public async Task<PassRPTestPartOneResponse> Handle(PassRPTestPartOneRequest request,
            CancellationToken cancellationToken)
        {
            var passRPTestPartOneResult = await rpTestManager.PassRPTestPartOne(request.AnswersDictionary)
                                          ?? throw new ServerException("Processing RP test failed");

            return new PassRPTestPartOneResponse {IsPassed = passRPTestPartOneResult.IsPassed};
        }
    }
}