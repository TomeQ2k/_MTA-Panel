using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record ToggleBlockCharacterRequest : IRequest<ToggleBlockCharacterResponse>
    {
        public int CharacterId { get; init; }
    }

    public class ToggleBlockCharacterRequestValidator : AbstractValidator<ToggleBlockCharacterRequest>
    {
        public ToggleBlockCharacterRequestValidator()
        {
            RuleFor(x => x.CharacterId).NotNull();
        }
    }
}