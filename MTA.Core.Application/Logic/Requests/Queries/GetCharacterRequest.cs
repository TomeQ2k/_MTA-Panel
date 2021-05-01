using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
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