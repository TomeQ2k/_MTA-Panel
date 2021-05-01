using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Common.Helpers;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record SendAddSerialEmailRequest : IRequest<SendAddSerialEmailResponse>
    {
        public string Serial { get; init; }
    }

    public class SendAddSerialEmailRequestValidator : AbstractValidator<SendAddSerialEmailRequest>
    {
        public SendAddSerialEmailRequestValidator()
        {
            RuleFor(x => x.Serial).NotNull().Matches(Constants.SerialRegex)
                .Length(Constants.SerialLength);
        }
    }
}