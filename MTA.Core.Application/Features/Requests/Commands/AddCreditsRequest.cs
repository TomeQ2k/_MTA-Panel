using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
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