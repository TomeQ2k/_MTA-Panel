using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record AddSerialRequest : IRequest<AddSerialResponse>
    {
        public string Serial { get; init; }
        public string Email { get; init; }
        public string Token { get; init; }
    }

    public class AddSerialRequestValidator : AbstractValidator<AddSerialRequest>
    {
        public AddSerialRequestValidator()
        {
            RuleFor(x => x.Serial).NotNull();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Token).NotNull();
        }
    }
}