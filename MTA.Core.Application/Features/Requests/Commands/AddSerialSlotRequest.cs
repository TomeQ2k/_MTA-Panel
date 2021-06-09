using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Models;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record AddSerialSlotRequest : IRequest<AddSerialSlotResponse>, IAmountable
    {
        public int Amount { get; init; }
    }

    public class AddSerialSlotRequestValidator : AbstractValidator<AddSerialSlotRequest>
    {
        public AddSerialSlotRequestValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}