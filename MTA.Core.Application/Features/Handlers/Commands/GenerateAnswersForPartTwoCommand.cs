using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Features.Handlers.Commands
{
    public class GenerateAnswersForPartTwoCommand : IRequestHandler<GenerateAnswersForPartTwoRequest, GenerateAnswersForPartTwoResponse>
    {
        private readonly IRPTestManager rpTestManager;

        public GenerateAnswersForPartTwoCommand(IRPTestManager rpTestManager)
        {
            this.rpTestManager = rpTestManager;
        }
        public async Task<GenerateAnswersForPartTwoResponse> Handle(GenerateAnswersForPartTwoRequest request, CancellationToken cancellationToken)
        => await rpTestManager.GenerateAnswersForPartTwo(request.AnswersDictionary)
        ? new GenerateAnswersForPartTwoResponse()
        : throw new ServerException("Processing RP test failed");
    }
}