using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record ChangePasswordRequest : IRequest<ChangePasswordResponse>
    {
        public string NewPassword { get; init; }
        public string Email { get; init; }
        public string Token { get; init; }
    }

    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword).NotNull();
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Token).NotNull();
        }
    }
}