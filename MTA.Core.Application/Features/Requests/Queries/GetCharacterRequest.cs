using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetCharacterRequest : IRequest<GetCharacterResponse>
    {
        public int CharacterId { get; init; }
    }

    public class GetCharacterRequestValidator : AbstractValidator<GetCharacterRequest>
    {
        public GetCharacterRequestValidator()
        {
            RuleFor(x => x.CharacterId).NotNull();
        }
    }
}