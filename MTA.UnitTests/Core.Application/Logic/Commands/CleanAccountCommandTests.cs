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
    public class CleanAccountCommandTests
    {
        private CleanAccountCommand cleanAccountCommand;

        private Mock<IUserManager> userManager;

        [SetUp]
        public void SetUp()
        {
            userManager = new Mock<IUserManager>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            userManager.Setup(u => u.CleanAccount(It.IsNotNull<int>())).ReturnsAsync(true);

            cleanAccountCommand = new CleanAccountCommand(userManager.Object, httpContextReader.Object, notifier.Object,
                hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CleaningAccountFailed_ThrowCrudException()
        {
            userManager.Setup(u => u.CleanAccount(It.IsNotNull<int>())).ReturnsAsync(false);

            Assert.That(
                () => cleanAccountCommand.Handle(new CleanAccountRequest {UserId = 1},
                    It.IsNotNull<CancellationToken>()), Throws.Exception.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCleanAccountResponse()
        {
            var result = await cleanAccountCommand.Handle(new CleanAccountRequest {UserId = 1},
                It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null
                .And.TypeOf<CleanAccountResponse>());
        }
    }
}