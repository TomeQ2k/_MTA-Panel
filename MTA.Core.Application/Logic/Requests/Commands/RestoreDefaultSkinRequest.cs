using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record RestoreDefaultSkinRequest : IRequest<RestoreDefaultSkinResponse>
    {
        public int CharacterId { get; init; }
    }

    public class RestoreDefaultSkinRequestValidator : AbstractValidator<RestoreDefaultSkinRequest>
    {
        public RestoreDefaultSkinRequestValidator()
        {
            RuleFor(x => x.CharacterId).NotNull();
        }
    }
}