using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;

namespace MTA.Core.Application.Logic.Handlers.Commands
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