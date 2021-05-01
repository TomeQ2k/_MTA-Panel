using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Common.Enums;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record DonateServerRequest : IRequest<DonateServerResponse>
    {
        public DonationType DonationType { get; init; }
        public string TokenCode { get; init; }
    }

    public class DonateServerRequestValidatior : AbstractValidator<DonateServerRequest>
    {
        public DonateServerRequestValidatior()
        {
            RuleFor(x => x.DonationType).IsInEnum();
            RuleFor(x => x.TokenCode).NotNull();
        }
    }
}