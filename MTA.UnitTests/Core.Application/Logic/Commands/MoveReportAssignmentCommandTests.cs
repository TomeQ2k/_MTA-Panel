using System.Collections.Generic;
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
using MTA.Core.Application.SignalR.Hubs;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class MoveReportAssignmentCommandTests
    {
        private Report report;
        private MoveReportAssignmentRequest request;

        private Mock<IReportManager> reportManager;
        private Mock<IReportValidationHub> reportValidationHub;
        private Mock<IHttpContextReader> httpContextReader;

        private MoveReportAssignmentCommand moveReportAssignmentCommand;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new MoveReportAssignmentRequest
            {
                ReportId = "xxx", NewAssigneeId = 0
            };

            reportManager = new Mock<IReportManager>();
            reportValidationHub = new Mock<IReportValidationHub>();
            httpContextReader = new Mock<IHttpContextReader>();
            var notifier = new Mock<INotifier>();
            var hubManager = new Mock<IHubManager<NotifierHub>>();
            var mapper = new Mock<IMapper>();

            reportManager.Setup(rm => rm.MoveReportAssignment(It.IsAny<Report>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            reportManager
                .Setup(rm => rm.GetReportsAllowedAssignees((ReportCategoryType) report.CategoryType, report.Private))
                .ReturnsAsync(new List<User>
                {
                    new User()
                });
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(1);

            moveReportAssignmentCommand =
                new MoveReportAssignmentCommand(reportManager.Object, reportValidationHub.Object,
                    httpContextReader.Object, notifier.Object, hubManager.Object, mapper.Object);
        }

        [Test]
        public void Handle_CurrentUserHasNoPermission_ThrowNoPermissionException()
        {
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => moveReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_NewAssigneeIsNotPermittedToBeReportAssignee_ThrowNoPermissionsException()
        {
            reportManager
                .Setup(rm => rm.GetReportsAllowedAssignees((ReportCategoryType) report.CategoryType, report.Private))
                .ReturnsAsync(new List<User>());

            Assert.That(() => moveReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_MovingReportAssignmentFailed_ThrowCrudException()
        {
            reportManager.Setup(rm => rm.MoveReportAssignment(It.IsAny<Report>(), It.IsAny<int>()))
                .ReturnsAsync(false);

            Assert.That(() => moveReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnMoveReportAssignmentResponse()
        {
            var result = await moveReportAssignmentCommand.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<MoveReportAssignmentResponse>());
        }
    }
}