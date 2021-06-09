using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record TransferCharacterRequest : IRequest<TransferCharacterResponse>
    {
        public int SourceCharacterId { get; init; }
        public int TargetCharacterId { get; init; }
    }

    public class TransferCharacterRequestValidator : AbstractValidator<TransferCharacterRequest>
    {
        public TransferCharacterRequestValidator()
        {
            RuleFor(x => x.SourceCharacterId).NotNull();
            RuleFor(x => x.TargetCharacterId).NotNull();
        }
    }
}