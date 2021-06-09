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
    public class CreateChangelogCommandTests
    {
        private Mock<IChangelogService> changelogService;
        private Mock<IMapper> mapper;
        private CreateChangelogCommand createChangelogCommand;

        [SetUp]
        public void SetUp()
        {
            changelogService = new Mock<IChangelogService>();
            mapper = new Mock<IMapper>();

            createChangelogCommand = new CreateChangelogCommand(changelogService.Object, mapper.Object);
        }

        [Test]
        public void Handle_InvalidRequest_ThrowCrudException()
        {
            changelogService.Setup(a => a.CreateChangelog(It.IsNotNull<CreateChangelogRequest>()))
                .ReturnsAsync(() => null);

            Assert.That(() => createChangelogCommand.Handle(It.IsNotNull<CreateChangelogRequest>(),
                It.IsNotNull<CancellationToken>()), Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_CreateChangelogResponse()
        {
            changelogService.Setup(a => a.CreateChangelog(It.IsAny<CreateChangelogRequest>()))
                .ReturnsAsync(new Changelog());
            mapper.Setup(m => m.Map<ChangelogDto>(It.IsAny<Changelog>())).Returns(new ChangelogDto());

            var result = await createChangelogCommand.Handle(It.IsAny<CreateChangelogRequest>(),
                It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.TypeOf<CreateChangelogResponse>());
            Assert.That(result.Changelog, Is.Not.Null.And.TypeOf<ChangelogDto>());
        }
    }
}