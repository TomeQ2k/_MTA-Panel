using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class DeleteArticleCommandTests
    {
        private Mock<IArticleService> articleService;
        private DeleteArticleCommand deleteArticleCommand;

        [SetUp]
        public void SetUp()
        {
            articleService = new Mock<IArticleService>();

            deleteArticleCommand = new DeleteArticleCommand(articleService.Object);
        }

        [Test]
        public void Handle_DeleteArticleFailed_ThrowCrudException()
        {
            var request = new DeleteArticleRequest();
            articleService.Setup(a => a.DeleteArticle(It.IsNotNull<string>()))
                .ReturnsAsync(false);

            Assert.That(
                () => deleteArticleCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnDeleteArticleResponse()
        {
            var request = new DeleteArticleRequest {ArticleId = "test"};
            articleService.Setup(a => a.DeleteArticle(It.IsNotNull<string>()))
                .ReturnsAsync(true);

            var result = await deleteArticleCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.TypeOf<DeleteArticleResponse>());
        }
    }
}