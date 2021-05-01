using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Commands;

namespace MTA.Core.Application.Logic.Requests.Commands
{
    public record DeleteArticleRequest : IRequest<DeleteArticleResponse>
    {
        public string ArticleId { get; init; }
    }

    public class DeleteArticleRequestValidator : AbstractValidator<DeleteArticleRequest>
    {
        public DeleteArticleRequestValidator()
        {
            RuleFor(x => x.ArticleId).NotNull();
        }
    }
}