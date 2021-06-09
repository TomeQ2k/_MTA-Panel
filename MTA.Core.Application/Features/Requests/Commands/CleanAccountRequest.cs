using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Commands;

namespace MTA.Core.Application.Features.Requests.Commands
{
    public record CleanAccountRequest : IRequest<CleanAccountResponse>
    {
        public int UserId { get; init; }
    }

    public class CleanAccountRequestValidator : AbstractValidator<CleanAccountRequest>
    {
        public CleanAccountRequestValidator()
        {
            RuleFor(x => x.UserId).NotNull();
        }
    }
}