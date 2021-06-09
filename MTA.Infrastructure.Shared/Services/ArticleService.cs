using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IDatabase database;
        private readonly IFilesManager filesManager;
        private readonly IMapper mapper;

        public ArticleService(IDatabase database, IFilesManager filesManager, IMapper mapper)
        {
            this.database = database;
            this.filesManager = filesManager;
            this.mapper = mapper;
        }

        public async Task<Article> GetArticle(int articleId)
            => await database.ArticleRepository.FindById(articleId)
               ?? throw new EntityNotFoundException("Article not found");

        public async Task<IEnumerable<Article>> GetArticles(int limit = Constants.ArticlesCount)
            => await database.ArticleRepository.FetchSortedArticles(limit);

        public async Task<Article> CreateArticle(CreateArticleRequest request)
        {
            var article = new ArticleBuilder()
                .SetTitle(request.Title)
                .SetContent(request.Content)
                .Build();

            if (!await database.ArticleRepository.Insert(article, false))
                throw new DatabaseException();

            if (request.Image != null)
            {
                await UploadImage(request.Image, article);
                await database.ArticleRepository.Update(article);
            }

            return article;
        }

        public async Task<Article> UpdateArticle(UpdateArticleRequest request)
        {
            var article = await database.ArticleRepository.FindById(request.ArticleId) ??
                          throw new EntityNotFoundException("Article not found");

            article = mapper.Map(request, article);

            if (request.IsImageDeleted && !string.IsNullOrEmpty(article.ImageUrl))
                await DeleteImage(article);
            else if (!request.IsImageDeleted && request.Image != null)
            {
                await DeleteImage(article);
                await UploadImage(request.Image, article);
            }

            return await database.ArticleRepository.Update(article) ? article : throw new DatabaseException();
        }

        public async Task<bool> DeleteArticle(string articleId)
        {
            if (!await database.ArticleRepository.DeleteByColumn(new("id", articleId)))
                throw new DatabaseException();

            filesManager.DeleteDirectory($"files/articles/{articleId}");

            return true;
        }

        #region private

        private async Task UploadImage(IFormFile image, Article article)
        {
            var uploadedPhoto = await filesManager.Upload(image, $"articles/{article.Id}");
            var articleImage = ArticleImage.Create(uploadedPhoto.Path, article.Id);

            await database.ArticleImageRepository.Insert(articleImage, false);

            article.SetImageUrl(uploadedPhoto.Path);
        }

        private async Task DeleteImage(Article article)
        {
            if (!await database.ArticleImageRepository.DeleteByColumn(new("articleId", article.Id)))
                throw new DatabaseException();

            filesManager.DeleteDirectory($"files/articles/{article.Id}");

            article.SetImageUrl(null);
        }

        #endregion
    }
}