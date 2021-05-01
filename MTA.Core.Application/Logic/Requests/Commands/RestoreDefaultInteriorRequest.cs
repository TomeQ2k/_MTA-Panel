using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record RestoreDefaultInteriorRequest : IRequest<RestoreDefaultInteriorResponse>
    {
        public int EstateId { get; init; }
    }

    public class RestoreDefaultInteriorRequestValidator : AbstractValidator<RestoreDefaultInteriorRequest>
    {
        public RestoreDefaultInteriorRequestValidator()
        {
            RuleFor(x => x.EstateId).NotNull();
        }
    }
}