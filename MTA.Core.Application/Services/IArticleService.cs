using System.Threading.Tasks;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface IArticleService : IReadOnlyArticleService
    {
        Task<Article> CreateArticle(CreateArticleRequest request);
        
        Task<Article> UpdateArticle(UpdateArticleRequest request);

        Task<bool> DeleteArticle(string articleId);
    }
}