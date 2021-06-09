using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
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