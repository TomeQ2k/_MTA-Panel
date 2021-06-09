using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record CreatePaymentRequest : IRequest<CreatePaymentResponse>
    {
        public decimal Price { get; init; }
    }

    public class CreatePaymentRequestValidator : AbstractValidator<CreatePaymentRequest>
    {
        public CreatePaymentRequestValidator()
        {
            RuleFor(x => x.Price).NotNull();
        }
    }
}