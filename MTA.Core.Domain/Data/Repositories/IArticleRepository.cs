using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface IArticleRepository : IRepository<Article>
    {
        Task<IEnumerable<Article>> FetchSortedArticles(int limit = Constants.ArticlesCount);
    }
}