using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class BlockAccountCommandTests
    {
        private BlockAccountCommand blockAccountCommand;

        private Mock<IUserManager> userManager;
        private BlockAccountRequest request;

        [SetUp]
        public void SetUp()
        {
            request = new BlockAccountRequest();

            userManager = new Mock<IUserManager>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();
            
            userManager.Setup(um => um.BlockAccount(It.IsNotNull<BlockAccountRequest>()))
                .ReturnsAsync(true);
            
            blockAccountCommand = new BlockAccountCommand(userManager.Object, httpContextReader.Object, notifier.Object, hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_BlockingAccountFailed_ThrowCrudException()
        {
            userManager.Setup(um => um.BlockAccount(It.IsNotNull<BlockAccountRequest>()))
                .ReturnsAsync(false);

            Assert.That(() => blockAccountCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnBlockAccountResponse()
        {
            var result = await blockAccountCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<BlockAccountResponse>());
            Assert.That(result.IsSucceeded, Is.True);
        }
    }
}