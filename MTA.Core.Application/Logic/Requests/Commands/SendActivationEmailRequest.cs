using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record SendActivationEmailRequest : IRequest<SendActivationEmailResponse>
    {
        public string Email { get; init; }
    }

    public class SendActivationEmailRequestValidator : AbstractValidator<SendActivationEmailRequest>
    {
        public SendActivationEmailRequestValidator()
        {
            RuleFor(x => x.Email).NotNull().IsEmailAdress()
                .Length(Constants.MinimumEmailLength, Constants.MaximumEmailLength);
        }
    }
}