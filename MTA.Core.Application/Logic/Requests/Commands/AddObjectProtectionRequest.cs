using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddObjectProtectionRequest : IRequest<AddObjectProtectionResponse>, IAmountable
    {
        public ObjectProtectionType ProtectionType { get; init; }
        public int ObjectId { get; init; }

        public int Amount { get; init; }
    }

    public class AddObjectProtectionRequestValidator : AbstractValidator<AddObjectProtectionRequest>
    {
        public AddObjectProtectionRequestValidator()
        {
            RuleFor(x => x.ProtectionType).IsInEnum();
            RuleFor(x => x.ObjectId).NotNull();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}