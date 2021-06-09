using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record CapturePaymentRequest : IRequest<CapturePaymentResponse>
    {
        public string Token { get; init; }
    }

    public class CapturePaymentRequestValidator : AbstractValidator<CapturePaymentRequest>
    {
        public CapturePaymentRequestValidator()
        {
            RuleFor(x => x.Token).NotNull();
        }
    }
}