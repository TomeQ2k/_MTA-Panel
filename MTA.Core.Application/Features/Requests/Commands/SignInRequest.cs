using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record SignInRequest : IRequest<SignInResponse>
    {
        public string Login { get; init; }
        public string Password { get; init; }
    }

    public class SignInRequestValidator : AbstractValidator<SignInRequest>
    {
        public SignInRequestValidator()
        {
            RuleFor(x => x.Login).NotNull().MaximumLength(Constants.MaximumEmailLength);
            RuleFor(x => x.Password).NotNull().MaximumLength(Constants.MaximumPasswordLength);
        }
    }
}