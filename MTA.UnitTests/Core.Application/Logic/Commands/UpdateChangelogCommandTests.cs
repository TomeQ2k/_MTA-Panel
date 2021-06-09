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
    public class UpdateChangelogCommandTests
    {
        private Mock<IChangelogService> changelogService;
        private Mock<IMapper> mapper;
        private UpdateChangelogCommand updateChangelogCommand;


        [SetUp]
        public void SetUp()
        {
            changelogService = new Mock<IChangelogService>();
            mapper = new Mock<IMapper>();

            updateChangelogCommand = new UpdateChangelogCommand(changelogService.Object, mapper.Object);
        }

        [Test]
        public void Handle_UpdatingChangelogFailed_ThrowCrudException()
        {
            changelogService.Setup(a => a.UpdateChangelog(It.IsAny<UpdateChangelogRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(
                () => updateChangelogCommand.Handle(new UpdateChangelogRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalledValid_ReturnUpdateChangelogResponse()
        {
            var changelog = new Changelog();
            changelogService.Setup(a => a.UpdateChangelog(It.IsAny<UpdateChangelogRequest>()))
                .ReturnsAsync(changelog);
            mapper.Setup(m => m.Map<ChangelogDto>(It.IsAny<Changelog>()))
                .Returns(new ChangelogDto());

            var result =
                await updateChangelogCommand.Handle(new UpdateChangelogRequest(), It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<UpdateChangelogResponse>());
            Assert.That(result.Changelog, Is.TypeOf<ChangelogDto>());
        }
    }
}