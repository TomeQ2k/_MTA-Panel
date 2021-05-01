using FluentValidation;
using MediatR;
using MTA.Core.Application.Logic.Responses.Queries;

namespace MTA.Core.Application.Logic.Requests.Queries
{
    public record GetArticleRequest : IRequest<GetArticleResponse>
    {
        public int ArticleId { get; init; }
    }

    public class GetArticleRequestValidator : AbstractValidator<GetArticleRequest>
    {
        public GetArticleRequestValidator()
        {
            RuleFor(x => x.ArticleId).NotNull();
        }
    }
}