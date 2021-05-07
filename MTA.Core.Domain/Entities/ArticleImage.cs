using MTA.Core.Domain.Data.Helpers;

namespace MTA.Core.Domain.Entities
{
    public class ArticleImage : BaseFile
    {
        [Column("articleId")] public string ArticleId { get; protected set; }

        public static ArticleImage Create(string path, string articleId) => new()
        {
            Path = path,
            ArticleId = articleId
        };
    }
}