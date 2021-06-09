using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Features.Requests.Queries;
using MTA.Core.Application.Features.Responses.Queries;
using MTA.Core.Application.Services.ReadOnly;

namespace MTA.Core.Application.Features.Handlers.Queries
{
    public class GetPartQuestionsQuery : IRequestHandler<GetPartQuestionsRequest, GetPartQuestionsResponse>
    {
        private readonly IReadOnlyRPTestManager rpTestManager;
        private readonly IMapper mapper;

        public GetPartQuestionsQuery(IReadOnlyRPTestManager rpTestManager, IMapper mapper)
        {
            this.rpTestManager = rpTestManager;
            this.mapper = mapper;
        }

        public async Task<GetPartQuestionsResponse> Handle(GetPartQuestionsRequest request,
            CancellationToken cancellationToken)
        {
            var partQuestions = await rpTestManager.GetPartQuestions(request.PartType);

            return new GetPartQuestionsResponse {PartQuestions = mapper.Map<IEnumerable<QuestionDto>>(partQuestions)};
        }
    }
}