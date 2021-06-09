using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Models;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private ArticleService articleService;

        private Mock<IDatabase> database;
        private Mock<IFilesManager> filesManager;
        private Mock<IMapper> mapper;

        private CreateArticleRequest createRequest;
        private UpdateArticleRequest updateRequest;

        private const string Id = "id";
        private const string ArticleId = "articleId";

        [SetUp]
        public void SetUp()
        {
            createRequest = new CreateArticleRequest()
            {
                Title = "title",
                Content = "content",
                Category = ArticleCategoryType.News,
                Image = It.IsAny<IFormFile>()
            };

            updateRequest = new UpdateArticleRequest()
            {
                ArticleId = It.IsAny<string>(),
                Title = "title",
                Content = "content",
                Category = ArticleCategoryType.News,
                Image = It.IsAny<IFormFile>(),
                IsImageDeleted = false
            };

            database = new Mock<IDatabase>();
            filesManager = new Mock<IFilesManager>();
            mapper = new Mock<IMapper>();

            articleService = new ArticleService(database.Object, filesManager.Object, mapper.Object);
        }

        #region CreateArticle

        [Test]
        public void CreateArticle_InsertingFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ArticleRepository.Insert(It.IsNotNull<Article>(), false))
                .ReturnsAsync(false);

            Assert.That(() => articleService.CreateArticle(createRequest),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateArticle_ImageUploadedAndInsertingSucceeded_ReturnArticle()
        {
            createRequest = createRequest with {Image = It.IsNotNull<IFormFile>()};

            database.Setup(d => d.ArticleRepository.Insert(It.IsNotNull<Article>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.ArticleRepository.Update(It.IsNotNull<Article>()))
                .ReturnsAsync(true);

            var result = await articleService.CreateArticle(createRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Article>());
        }

        [Test]
        public async Task CreateArticle_ImageNotUploadedAndInsertingSucceeded_ReturnArticle()
        {
            database.Setup(d => d.ArticleRepository.Insert(It.IsNotNull<Article>(), false))
                .ReturnsAsync(true);

            var result = await articleService.CreateArticle(createRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Article>());
        }

        #endregion

        #region UpdateArticle

        [Test]
        public void UpdateArticle_ArticleNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.ArticleRepository.FindById(updateRequest.ArticleId))
                .ReturnsAsync(() => null);

            Assert.That(() => articleService.UpdateArticle(updateRequest),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task UpdateArticle_ImageIsDeleted_DeleteImageShouldBeCalled()
        {
            updateRequest = updateRequest with {IsImageDeleted = true, Image = It.IsNotNull<IFormFile>()};

            var article = SetUpUpdateArticleWhenImageIsDeleted();
            database.Setup(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id))).ReturnsAsync(true);

            await articleService.UpdateArticle(updateRequest);

            database.Verify(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id)));
            filesManager.Verify(fm => fm.DeleteDirectory(It.IsAny<string>(), true));
        }

        [Test]
        public void UpdateArticle_DeletingImageFailed_ThrowDatabaseException()
        {
            updateRequest = updateRequest with {IsImageDeleted = true, Image = It.IsNotNull<IFormFile>()};

            var article = SetUpUpdateArticleWhenImageIsDeleted();
            database.Setup(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id)))
                .ReturnsAsync(false);

            Assert.That(() => articleService.UpdateArticle(updateRequest),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task UpdateArticle_ImageIsChanged_DeleteImageAndInsertImageShouldBeCalled(
            bool imageInserted)
        {
            updateRequest = updateRequest with
            {
                IsImageDeleted = false, Image = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };

            var article = SetUpUpdateArticleWhenImageIsChanged(imageInserted);
            database.Setup(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id))).ReturnsAsync(true);

            await articleService.UpdateArticle(updateRequest);

            database.Verify(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id)));
            filesManager.Verify(fm => fm.DeleteDirectory(It.IsAny<string>(), true));
            filesManager.Verify(fm => fm.Upload(It.IsNotNull<IFormFile>(), It.IsNotNull<string>()));
            database.Verify(d => d.ArticleImageRepository.Insert(It.IsNotNull<ArticleImage>(), false));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void UpdateArticle_DuringChangingImageDeletingFailed_ThrowDatabaseException(bool imageInserted)
        {
            updateRequest = updateRequest with
            {
                IsImageDeleted = false, Image = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };

            var article = SetUpUpdateArticleWhenImageIsChanged(imageInserted);
            database.Setup(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id)))
                .ReturnsAsync(false);

            Assert.That(() => articleService.UpdateArticle(updateRequest),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task UpdateArticle_ImageIsDeleted_ReturnArticle()
        {
            updateRequest = updateRequest with {IsImageDeleted = true, Image = It.IsNotNull<IFormFile>()};

            var article = SetUpUpdateArticleWhenImageIsDeleted();
            database.Setup(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id))).ReturnsAsync(true);

            var result = await articleService.UpdateArticle(updateRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Article>());
            Assert.That(result.Id, Is.EqualTo(article.Id));
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task UpdateArticle_ImageIsChanged_ReturnArticle(bool imageInserted)
        {
            updateRequest = updateRequest with
            {
                IsImageDeleted = false, Image = new FormFile(It.IsNotNull<Stream>(), It.IsNotNull<long>(),
                    It.IsNotNull<long>(),
                    It.IsNotNull<string>(), It.IsNotNull<string>())
            };

            var article = SetUpUpdateArticleWhenImageIsChanged(imageInserted);
            database.Setup(d => d.ArticleImageRepository.DeleteByColumn(new(ArticleId, article.Id))).ReturnsAsync(true);

            var result = await articleService.UpdateArticle(updateRequest);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Article>());
            Assert.That(result.Id, Is.EqualTo(article.Id));
        }

        #endregion

        #region DeleteArticle

        [Test]
        public void DeleteArticle_DeletingFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ArticleRepository.DeleteByColumn(new(Id, It.IsNotNull<string>())))
                .ReturnsAsync(false);

            Assert.That(() => articleService.DeleteArticle(It.IsNotNull<string>()),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task DeleteArticle_ArticleIsDeleted_ImageShouldBeDeleted()
        {
            database.Setup(d => d.ArticleRepository.DeleteByColumn(new(Id, It.IsNotNull<string>()))).ReturnsAsync(true);

            await articleService.DeleteArticle(It.IsNotNull<string>());

            filesManager.Verify(fm => fm.DeleteDirectory(It.IsNotNull<string>(), true));
        }

        [Test]
        public async Task DeleteArticle_ArticleIsDeleted_ReturnTrue()
        {
            database.Setup(d => d.ArticleRepository.DeleteByColumn(new(Id, It.IsNotNull<string>()))).ReturnsAsync(true);

            var result = await articleService.DeleteArticle(It.IsNotNull<string>());

            Assert.That(result, Is.True);
        }

        #endregion

        #region private

        private Article SetUpUpdateArticleWhenImageIsDeleted()
        {
            var article = new ArticleBuilder()
                .SetTitle(It.IsNotNull<string>())
                .SetContent(It.IsNotNull<string>())
                .SetImageUrl("url")
                .Build();

            database.Setup(d => d.ArticleRepository.FindById(updateRequest.ArticleId)).ReturnsAsync(article);
            mapper.Setup(m => m.Map(updateRequest, article)).Returns(article);
            database.Setup(d => d.ArticleRepository.Update(article)).ReturnsAsync(true);

            return article;
        }

        private Article SetUpUpdateArticleWhenImageIsChanged(bool imageInserted)
        {
            var article = new ArticleBuilder()
                .SetTitle(It.IsNotNull<string>())
                .SetContent(It.IsNotNull<string>())
                .SetImageUrl(It.IsAny<string>())
                .Build();

            database.Setup(d => d.ArticleRepository.FindById(updateRequest.ArticleId)).ReturnsAsync(article);
            mapper.Setup(m => m.Map(updateRequest, article)).Returns(article);
            filesManager.Setup(fm => fm.Upload(It.IsNotNull<IFormFile>(), It.IsNotNull<string>()))
                .ReturnsAsync(new FileModel("path", "url", 1));
            database.Setup(d => d.ArticleImageRepository.Insert(It.IsNotNull<ArticleImage>(), false))
                .ReturnsAsync(imageInserted);
            database.Setup(d => d.ArticleRepository.Update(article)).ReturnsAsync(true);

            return article;
        }

        #endregion
    }
}