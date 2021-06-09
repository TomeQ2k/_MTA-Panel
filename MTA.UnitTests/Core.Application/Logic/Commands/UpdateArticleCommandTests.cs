using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Dtos;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class UpdateArticleCommandTests
    {
        private Mock<IArticleService> articleService;
        private Mock<IMapper> mapper;
        private UpdateArticleCommand updateArticleCommand;

        [SetUp]
        public void SetUp()
        {
            articleService = new Mock<IArticleService>();
            mapper = new Mock<IMapper>();

            updateArticleCommand = new UpdateArticleCommand(articleService.Object, mapper.Object);
        }

        [Test]
        public void Handle_UpdatingArticleFailed_ThrowCrudException()
        {
            articleService.Setup(a => a.UpdateArticle(It.IsAny<UpdateArticleRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => updateArticleCommand.Handle(new UpdateArticleRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalledValid_ReturnUpdateAricleResponse()
        {
            var article = new Article();
            articleService.Setup(a => a.UpdateArticle(It.IsAny<UpdateArticleRequest>()))
                .ReturnsAsync(article);
            mapper.Setup(m => m.Map<ArticleDto>(It.IsAny<Article>()))
                .Returns(new ArticleDto());

            var result = await updateArticleCommand.Handle(new UpdateArticleRequest(), It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<UpdateArticleResponse>());
            Assert.That(result.Article, Is.TypeOf<ArticleDto>());
        }
    }
}