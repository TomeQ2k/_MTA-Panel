using MTA.Core.Application.Models;

namespace MTA.Core.Application.Logic.Responses.Commands
{
    public record GenerateAnswersForPartTwoResponse : BaseResponse
    {
        public GenerateAnswersForPartTwoResponse(Error error = null) : base(error)
        {
        }
    }
}