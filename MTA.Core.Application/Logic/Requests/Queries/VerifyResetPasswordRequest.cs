using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record VerifyResetPasswordRequest : IRequest<VerifyResetPasswordResponse>
    {
        public string Email { get; init; }
        public string Token { get; init; }
    }

    public class VerifyResetPasswordRequestValidator : AbstractValidator<VerifyResetPasswordRequest>
    {
        public VerifyResetPasswordRequestValidator()
        {
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Token).NotNull();
        }
    }
}