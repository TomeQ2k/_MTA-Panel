using System.Collections.Generic;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record GenerateAnswersForPartTwoRequest : IRequest<GenerateAnswersForPartTwoResponse>
    {
        public Dictionary<int, string> AnswersDictionary { get; init; }
    }

    public class GenerateAnswersForPartTwoRequestValidator : AbstractValidator<GenerateAnswersForPartTwoRequest>
    {
        public GenerateAnswersForPartTwoRequestValidator()
        {
            RuleFor(x => x.AnswersDictionary).NotNull().NotEmpty();
        }
    }
}