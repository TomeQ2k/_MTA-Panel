using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record DeleteChangelogRequest : IRequest<DeleteChangelogResponse>
    {
        public string ChangelogId { get; init; }
    }

    public class DeleteChangelogRequestValidator : AbstractValidator<DeleteChangelogRequest>
    {
        public DeleteChangelogRequestValidator()
        {
            RuleFor(x => x.ChangelogId).NotNull();
        }
    }
}