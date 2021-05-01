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
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AddReportSubscriberCommandTests
    {
        private AddReportSubscriberCommand addReportSubscriberCommand;

        private Mock<IReportSubscriberService> reportSubscriberService;
        private Mock<IReportValidationHub> reportValidationHub;
        private Mock<IReportManager> reportManager;
        private Mock<IHttpContextReader> httpContextReader;
        private Mock<IMapper> mapper;

        private AddReportSubscriberRequest request;
        private Report report;
        private ReportSubscriber reportSubscriber;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            request = new AddReportSubscriberRequest {UserId = UserId};

            report = new Report();
            reportSubscriber = new ReportSubscriber();

            reportSubscriberService = new Mock<IReportSubscriberService>();
            reportValidationHub = new Mock<IReportValidationHub>();
            reportManager = new Mock<IReportManager>();
            httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            mapper = new Mock<IMapper>();

            reportSubscriberService.Setup(rss => rss.AddSubscriber(report, UserId))
                .ReturnsAsync(reportSubscriber);
            reportValidationHub.Setup(rvh =>
                    rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);
            reportManager.Setup(rm => rm.ChangeStatus(It.IsAny<ReportStatusType>(), report)).ReturnsAsync(true);
            httpContextReader.Setup(htc => htc.CurrentUserId).Returns(UserId);

            addReportSubscriberCommand = new AddReportSubscriberCommand(reportSubscriberService.Object,
                reportValidationHub.Object, reportManager.Object, httpContextReader.Object, notifier.Object,
                hubManager.Object,
                mapper.Object);
        }

        [Test]
        public void Handle_ReportValidationFailed_ThrowNoPermissionsException()
        {
            reportValidationHub.Setup(
                    rvh => rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => addReportSubscriberCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_AddingReportSubscriberFailed_ThrowCrudException()
        {
            reportSubscriberService.Setup(rss => rss.AddSubscriber(report, UserId))
                .ReturnsAsync(() => null);

            Assert.That(() => addReportSubscriberCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnAddReportSubscriberResponse()
        {
            var result = await addReportSubscriberCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result, Is.TypeOf<AddReportSubscriberResponse>());
        }
    }
}