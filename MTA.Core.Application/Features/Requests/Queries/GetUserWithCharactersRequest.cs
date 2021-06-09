using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
{
    public record GetUserWithCharactersRequest : IRequest<GetUserWithCharactersResponse>
    {
        public int UserId { get; init; }
    }

    public class GetUserWithCharactersRequestValidator : AbstractValidator<GetUserWithCharactersRequest>
    {
        public GetUserWithCharactersRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull();
        }
    }
}