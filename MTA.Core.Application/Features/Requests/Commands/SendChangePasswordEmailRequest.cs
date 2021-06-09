using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record SendChangePasswordEmailRequest : IRequest<SendChangePasswordEmailResponse>
    {
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }
    }

    public class SendChangePasswordEmailRequestValidator : AbstractValidator<SendChangePasswordEmailRequest>
    {
        public SendChangePasswordEmailRequestValidator()
        {
            RuleFor(x => x.OldPassword).NotNull()
                .Length(Constants.MinimumPasswordLength, Constants.MaximumPasswordLength);
            RuleFor(x => x.NewPassword).NotNull()
                .Length(Constants.MinimumPasswordLength, Constants.MaximumPasswordLength);
        }
    }
}