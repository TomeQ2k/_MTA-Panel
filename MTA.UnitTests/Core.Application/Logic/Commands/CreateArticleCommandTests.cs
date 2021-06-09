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
    public class CreateArticleCommandTests
    {
        private Mock<IArticleService> articleService;
        private Mock<IMapper> mapper;
        private CreateArticleCommand createArticleCommand;

        [SetUp]
        public void SetUp()
        {
            articleService = new Mock<IArticleService>();
            mapper = new Mock<IMapper>();

            createArticleCommand = new CreateArticleCommand(articleService.Object, mapper.Object);
        }

        [Test]
        public void Handle_InvalidRequest_ThrowCrudException()
        {
            articleService.Setup(a => a.CreateArticle(It.IsNotNull<CreateArticleRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createArticleCommand.Handle(It.IsNotNull<CreateArticleRequest>(),
                It.IsNotNull<CancellationToken>()), Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCreateArticleResponse()
        {
            articleService.Setup(a => a.CreateArticle(It.IsAny<CreateArticleRequest>()))
                .ReturnsAsync(new Article());

            mapper.Setup(m => m.Map<ArticleDto>(It.IsAny<Article>())).Returns(new ArticleDto());

            var result = await createArticleCommand.Handle(It.IsAny<CreateArticleRequest>(),
                It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.TypeOf<CreateArticleResponse>());
            Assert.That(result.Article, Is.Not.Null.And.TypeOf<ArticleDto>());
        }
    }
}