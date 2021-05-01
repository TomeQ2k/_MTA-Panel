using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyArticleService
    {
        Task<Article> GetArticle(int articleId);
        Task<IEnumerable<Article>> GetArticles(int limit = Constants.ArticlesCount);
    }
}