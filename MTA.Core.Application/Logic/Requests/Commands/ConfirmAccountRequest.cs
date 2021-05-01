using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record ConfirmAccountRequest : IRequest<ConfirmAccountResponse>
    {
        public string Email { get; init; }
        public string Token { get; init; }
    }

    public class ConfirmAccountRequestValidator : AbstractValidator<ConfirmAccountRequest>
    {
        public ConfirmAccountRequestValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Token).NotNull();
        }
    }
}