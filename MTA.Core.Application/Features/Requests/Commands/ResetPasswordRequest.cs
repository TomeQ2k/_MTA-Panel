using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record ResetPasswordRequest : IRequest<ResetPasswordResponse>
    {
        public string Email { get; init; }
        public string Token { get; init; }
        public string NewPassword { get; init; }
    }

    public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Token).NotNull();
            RuleFor(x => x.NewPassword).NotNull()
                .Length(Constants.MinimumPasswordLength, Constants.MaximumPasswordLength);
        }
    }
}