using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record ChangeEmailRequest : IRequest<ChangeEmailResponse>
    {
        public string NewEmail { get; init; }
        public string Email { get; init; }
        public string Token { get; init; }
    }

    public class ChangeEmailRequestValidator : AbstractValidator<ChangeEmailRequest>
    {
        public ChangeEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail).NotNull();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Token).NotNull();
        }
    }
}