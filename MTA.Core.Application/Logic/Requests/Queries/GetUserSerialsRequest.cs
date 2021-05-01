using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetUserSerialsRequest : IRequest<GetUserSerialsResponse>
    {
        public int UserId { get; init; }
    }

    public class GetUserSerialsRequestValidator : AbstractValidator<GetUserSerialsRequest>
    {
        public GetUserSerialsRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull();
        }
    }
}