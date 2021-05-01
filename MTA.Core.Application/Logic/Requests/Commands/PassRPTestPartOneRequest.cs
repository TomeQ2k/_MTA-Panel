using System.Collections.Generic;
using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record PassRPTestPartOneRequest : IRequest<PassRPTestPartOneResponse>
    {
        public Dictionary<int, int> AnswersDictionary { get; init; }
    }

    public class PassRPTestPartOneRequestValidator : AbstractValidator<PassRPTestPartOneRequest>
    {
        public PassRPTestPartOneRequestValidator()
        {
            RuleFor(x => x.AnswersDictionary).NotNull().NotEmpty();
        }
    }
}