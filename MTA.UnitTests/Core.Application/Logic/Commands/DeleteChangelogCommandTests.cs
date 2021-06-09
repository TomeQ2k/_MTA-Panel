using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class DeleteChangelogCommandTests
    {
        private Mock<IChangelogService> changelogService;
        private DeleteChangelogCommand deleteChangelogCommand;

        [SetUp]
        public void SetUp()
        {
            changelogService = new Mock<IChangelogService>();

            deleteChangelogCommand = new DeleteChangelogCommand(changelogService.Object);
        }

        [Test]
        public void Handle_DeleteChangelogFailed_ThrowCrudException()
        {
            var request = new DeleteChangelogRequest();
            changelogService.Setup(a => a.DeleteChangelog(It.IsNotNull<string>()))
                .ReturnsAsync(false);

            Assert.That(
                () => deleteChangelogCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnDeleteChangelogResponse()
        {
            var request = new DeleteChangelogRequest {ChangelogId = "test"};
            changelogService.Setup(a => a.DeleteChangelog(It.IsNotNull<string>()))
                .ReturnsAsync(true);

            var result = await deleteChangelogCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.TypeOf<DeleteChangelogResponse>());
        }
    }
}