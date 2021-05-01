using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AddCreditsCommandTests
    {
        private Mock<IUserManager> userManager;
        private Mock<INotifier> notifier;
        private Mock<IHubManager<NotifierHub>> hubManager;
        private Mock<IMapper> mapper;
        private AddCreditsCommand addCreditsCommand;

        [SetUp]
        public void SetUp()
        {
            userManager = new Mock<IUserManager>();
            notifier = new Mock<INotifier>();
            hubManager = new Mock<IHubManager<NotifierHub>>();
            mapper = new Mock<IMapper>();
            var httpContextReader = new Mock<IHttpContextReader>();
            
            addCreditsCommand =
                new AddCreditsCommand(userManager.Object, notifier.Object, hubManager.Object, httpContextReader.Object,mapper.Object);
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddCreditsResponse()
        {
            var acResult = new AddCreditsResult(1);

            userManager.Setup(um => um.AddCredits(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(acResult);

            var result = await addCreditsCommand.Handle(new AddCreditsRequest(), It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<AddCreditsResponse>().And.Not.Null);
            Assert.That(result.CreditsCount, Is.EqualTo(acResult.CurrentCreditsCount));
        }
    }
}