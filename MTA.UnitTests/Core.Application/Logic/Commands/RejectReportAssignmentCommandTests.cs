using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Logic.Handlers.Commands;
using MTA.Core.Application.Logic.Requests.Commands;
using MTA.Core.Application.Logic.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class RejectReportAssignmentCommandTests
    {
        private Report report;
        private RejectReportAssignmentRequest request;

        private Mock<IReportManager> reportManager;
        private Mock<IReportValidationHub> reportValidationHub;

        private RejectReportAssignmentCommand rejectReportAssignmentCommand;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new RejectReportAssignmentRequest
            {
                ReportId = "xxx"
            };

            reportManager = new Mock<IReportManager>();
            reportValidationHub = new Mock<IReportValidationHub>();

            reportManager.Setup(rm => rm.MoveReportAssignment(It.IsAny<Report>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);

            rejectReportAssignmentCommand =
                new RejectReportAssignmentCommand(reportManager.Object, reportValidationHub.Object);
        }

        [Test]
        public void Handle_CurrentUserHasNoPermission_ThrowNoPermissionException()
        {
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => rejectReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(true, false)]
        public void Handle_AcceotReportAssignmentFailed_ThrowCrudException(bool isAccepted, bool isSucceeded)
        {
            SetUpReportAssignmentResult(isAccepted, isSucceeded);

            Assert.That(() => rejectReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task Handle_WhenCalled_ReturnAcceptReportAssignmentResponse(bool isAccepted, bool isSucceeded)
        {
            SetUpReportAssignmentResult(isAccepted, isSucceeded);

            var result = await rejectReportAssignmentCommand.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<RejectReportAssignmentResponse>());
            Assert.That(result.IsAccepted, Is.EqualTo(isAccepted));
            Assert.That(result.IsSucceeded, Is.EqualTo(isSucceeded));
        }

        #region private

        private void SetUpReportAssignmentResult(bool isAccepted, bool isSucceeded)
        {
            var reportAssignmentResult = new ReportAssignmentResult(isAccepted, isSucceeded);

            reportManager.Setup(rm => rm.RejectReportAssignment(It.IsAny<Report>()))
                .ReturnsAsync(reportAssignmentResult);
        }

        #endregion
    }
}