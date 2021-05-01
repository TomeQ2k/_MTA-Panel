using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory, table)
        {
        }

        public async Task<IEnumerable<Article>> FetchSortedArticles(int limit = Constants.ArticlesCount)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .OrderBy("dateCreated", OrderByType.Descending)
                .Limit(limit)
                .Build());
    }
}