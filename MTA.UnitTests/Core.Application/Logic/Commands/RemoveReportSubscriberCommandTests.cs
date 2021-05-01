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
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class RemoveReportSubscriberCommandTests
    {
        private RemoveReportSubscriberCommand removeReportSubscriberCommand;

        private Mock<IReportSubscriberService> reportSubscriberService;
        private Mock<IReportValidationHub> reportValidationHub;

        private RemoveReportSubscriberRequest request;
        private Report report;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            request = new RemoveReportSubscriberRequest {UserId = UserId};

            report = new Report();

            reportSubscriberService = new Mock<IReportSubscriberService>();
            reportValidationHub = new Mock<IReportValidationHub>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            reportSubscriberService.Setup(rss => rss.RemoveSubscriber(report, UserId))
                .ReturnsAsync(true);
            reportValidationHub.Setup(rvh =>
                    rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);

            removeReportSubscriberCommand =
                new RemoveReportSubscriberCommand(reportSubscriberService.Object, reportValidationHub.Object,
                    httpContextReader.Object, notifier.Object, hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_ReportValidationFailed_ThrowNoPermissionsException()
        {
            reportValidationHub.Setup(
                    rvh => rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => removeReportSubscriberCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_RemovingReportSubscriberFailed_ThrowCrudException()
        {
            reportSubscriberService.Setup(rss => rss.RemoveSubscriber(report, UserId))
                .ReturnsAsync(false);

            Assert.That(() => removeReportSubscriberCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddReportSubscriberResponse()
        {
            var result = await removeReportSubscriberCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result, Is.TypeOf<RemoveReportSubscriberResponse>());
        }
    }
}