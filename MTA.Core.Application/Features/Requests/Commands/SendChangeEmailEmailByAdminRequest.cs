using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Validation.Validators;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record SendChangeEmailEmailByAdminRequest : IRequest<SendChangeEmailEmailByAdminResponse>
    {
        public int UserId { get; init; }
        public string NewEmail { get; init; }
    }

    public class SendChangeEmailEmailByAdminRequestValidator : AbstractValidator<SendChangeEmailEmailByAdminRequest>
    {
        public SendChangeEmailEmailByAdminRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull();
            RuleFor(x => x.NewEmail).NotNull().IsEmailAdress()
                .Length(Constants.MinimumEmailLength, Constants.MaximumEmailLength);
        }
    }
}