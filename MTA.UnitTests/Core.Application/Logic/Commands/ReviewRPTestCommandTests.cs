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
using MTA.Core.Common.Enums;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class ReviewRPTestCommandTests
    {
        private Mock<IRPTestManager> rpTestManager;
        private ReviewRPTestCommand reviewRPTestCommand;

        [SetUp]
        public void SetUp()
        {
            rpTestManager = new Mock<IRPTestManager>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            rpTestManager.Setup(r =>
                    r.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationStateType>()))
                .ReturnsAsync(new ReviewTestResult(true, It.IsNotNull<int>()));

            reviewRPTestCommand = new ReviewRPTestCommand(rpTestManager.Object, httpContextReader.Object,
                notifier.Object, hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_ReviewFailed_ThrowServerException()
        {
            rpTestManager.Setup(r =>
                    r.ReviewRPTest(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<ApplicationStateType>()))
                .ReturnsAsync(new ReviewTestResult(false, It.IsNotNull<int>()));

            Assert.That(() => reviewRPTestCommand.Handle(new ReviewRPTestRequest(), It.IsAny<CancellationToken>()),
                Throws.TypeOf<ServerException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnReviewRPTestResponse()
        {
            var result = await reviewRPTestCommand.Handle(new ReviewRPTestRequest(), It.IsAny<CancellationToken>());

            Assert.That(result, Is.TypeOf<ReviewRPTestResponse>());
        }
    }
}