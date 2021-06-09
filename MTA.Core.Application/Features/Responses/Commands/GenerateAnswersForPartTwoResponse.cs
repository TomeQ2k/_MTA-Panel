using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Responses.Commands
{
    public record GenerateAnswersForPartTwoResponse : BaseResponse
    {
        public GenerateAnswersForPartTwoResponse(Error error = null) : base(error)
        {
        }
    }
}