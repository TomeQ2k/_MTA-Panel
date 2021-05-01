using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddCreditsRequest : IRequest<AddCreditsResponse>
    {
        public int Credits { get; init; }
        public int UserId { get; init; }
    }

    public class AddCreditsRequestValidator : AbstractValidator<AddCreditsRequest>
    {
        public AddCreditsRequestValidator()
        {
            RuleFor(x => x.Credits).NotNull();
            RuleFor(x => x.UserId).NotNull();
        }
    }
}