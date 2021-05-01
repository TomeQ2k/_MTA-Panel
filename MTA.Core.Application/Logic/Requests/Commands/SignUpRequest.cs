using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record SignUpRequest : IRequest<SignUpResponse>
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
        public string Serial { get; init; }
        public string Referrer { get; init; }
        public string CaptchaResponse { get; init; }
    }

    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
            RuleFor(x => x.Username).NotNull().Matches(Constants.UsernameRegex)
                .Length(Constants.MinimumUsernameLength, Constants.MaximumUsernameLength);
            RuleFor(x => x.Email).NotNull().IsEmailAdress()
                .Length(Constants.MinimumEmailLength, Constants.MaximumEmailLength);
            RuleFor(x => x.Password).NotNull()
                .Length(Constants.MinimumPasswordLength, Constants.MaximumPasswordLength);
            RuleFor(x => x.Serial).NotNull().Matches(Constants.SerialRegex)
                .Length(Constants.SerialLength);
            RuleFor(x => x.Referrer).Matches(Constants.UsernameRegex)
                .MaximumLength(Constants.MaximumUsernameLength);
            RuleFor(x => x.CaptchaResponse).NotNull();
        }
    }
}