using FluentValidation;
using MediatR;
using MTA.Core.Application.Features.Responses.Queries;

namespace MTA.Core.Application.Features.Requests.Queries
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