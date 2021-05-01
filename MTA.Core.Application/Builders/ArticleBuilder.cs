using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class ArticleBuilder : IArticleBuilder
    {
        private readonly Article article = new();

        public IArticleBuilder SetTitle(string title)
        {
            article.SetTitle(title);
            return this;
        }

        public IArticleBuilder SetContent(string content)
        {
            article.SetContent(content);
            return this;
        }

        public IArticleBuilder SetImageUrl(string imageUrl)
        {
            article.SetImageUrl(imageUrl);
            return this;
        }

        public IArticleBuilder InCategory(ArticleCategoryType categoryType)
        {
            article.SetCategory(categoryType);
            return this;
        }

        public Article Build() => article;
    }
}