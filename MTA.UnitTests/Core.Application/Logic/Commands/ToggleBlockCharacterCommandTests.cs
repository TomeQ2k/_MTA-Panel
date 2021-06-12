using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Application.SignalR.Hubs;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class ToggleBlockCharacterCommandTests
    {
        private ToggleBlockCharacterCommand toggleBlockCharacterCommand;

        private Mock<ICharacterManager> characterManager;

        private ToggleBlockCharacterRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new ToggleBlockCharacterRequest();

            characterManager = new Mock<ICharacterManager>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            characterManager.Setup(cm => cm.ToggleBlockCharacter(request.CharacterId))
                .ReturnsAsync(new BlockCharacterResult(true, It.IsNotNull<bool>(), "test", 1));

            toggleBlockCharacterCommand =
                new ToggleBlockCharacterCommand(characterManager.Object, httpContextReader.Object, notifier.Object,
                    hubManager.Object, mapper.Object);
        }

        [Test]
        public async Task Handle_CharacterBlockStatusToggled_ReturnResponse()
        {
            var result = await toggleBlockCharacterCommand.Handle(request,
                It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ToggleBlockCharacterResponse>());
        }

        [Test]
        public void Handle_TogglingCharacterBlockStatusFailed_ThrowCrudException()
        {
            characterManager.Setup(cm => cm.ToggleBlockCharacter(request.CharacterId))
                .ReturnsAsync(new BlockCharacterResult(false, It.IsNotNull<bool>(), "test", 1));

            Assert.That(() => toggleBlockCharacterCommand.Handle(request,
                It.IsNotNull<CancellationToken>()), Throws.Exception.TypeOf<CrudException>());
        }
    }
}