using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record SendChangeEmailEmailRequest : IRequest<SendChangeEmailEmailResponse>
    {
        public string NewEmail { get; init; }
    }

    public class SendChangeEmailEmailRequestValidator : AbstractValidator<SendChangeEmailEmailRequest>
    {
        public SendChangeEmailEmailRequestValidator()
        {
            RuleFor(x => x.NewEmail).NotNull().IsEmailAdress()
                .Length(Constants.MinimumEmailLength, Constants.MaximumEmailLength);
        }
    }
}