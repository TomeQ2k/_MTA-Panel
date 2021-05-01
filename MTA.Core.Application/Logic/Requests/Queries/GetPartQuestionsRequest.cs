using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetPartQuestionsRequest : IRequest<GetPartQuestionsResponse>
    {
        public RPTestPartType PartType { get; init; }
    }

    public class GetPartQuestionsRequestValidator : AbstractValidator<GetPartQuestionsRequest>
    {
        public GetPartQuestionsRequestValidator()
        {
            RuleFor(x => x.PartType).IsInEnum();
        }
    }
}