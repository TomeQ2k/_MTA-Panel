using MTA.Core.Common.Enums;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface IArticleBuilder : IBuilder<Article>
    {
        IArticleBuilder SetTitle(string title);
        IArticleBuilder SetContent(string content);
        IArticleBuilder SetImageUrl(string imageUrl);
        IArticleBuilder InCategory(ArticleCategoryType categoryType);
    }
}