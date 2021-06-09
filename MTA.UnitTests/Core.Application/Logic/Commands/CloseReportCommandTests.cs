using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Application.SignalR;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class CloseReportCommandTests
    {
        private CloseReportCommand closeReportCommand;

        private Mock<IReportManager> reportManager;
        private Mock<IReportValidationHub> reportValidationHub;

        private CloseReportRequest request;
        private Report report;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new CloseReportRequest
            {
                ReportId = "xxx"
            };

            reportManager = new Mock<IReportManager>();
            reportValidationHub = new Mock<IReportValidationHub>();
            var httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            reportManager.Setup(rm => rm.CloseReport(It.IsAny<Report>()))
                .ReturnsAsync(true);
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);

            closeReportCommand = new CloseReportCommand(reportManager.Object, reportValidationHub.Object,
                httpContextReader.Object, notifier.Object, hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CurrentUserHasNoPermission_ThrowNoPermissionException()
        {
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => closeReportCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_ClosingReportFailed_ThrowCrudException()
        {
            reportManager.Setup(rm => rm.CloseReport(It.IsAny<Report>()))
                .ReturnsAsync(false);

            Assert.That(() => closeReportCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnCloseReportResponse()
        {
            var result = await closeReportCommand.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<CloseReportResponse>());
        }
    }
}