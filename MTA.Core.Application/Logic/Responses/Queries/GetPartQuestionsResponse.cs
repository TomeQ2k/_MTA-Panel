using System.Collections.Generic;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Queries
{
    public record GetPartQuestionsResponse : BaseResponse
    {
        public IEnumerable<QuestionDto> PartQuestions { get; init; }

        public GetPartQuestionsResponse(Error error = null) : base(error)
        {
        }
    }
}