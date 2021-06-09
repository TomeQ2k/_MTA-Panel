using System.Threading;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Handlers.Commands;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Features.Responses.Commands;
using MTA.Core.Application.Results;
using MTA.Core.Application.Services;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using NUnit.Framework;

namespace MTA.UnitTests.Core.Application.Logic.Commands
{
    [TestFixture]
    public class AcceptReportAssignmentCommandTests
    {
        private Report report;
        private AcceptReportAssignmentRequest request;

        private Mock<IReportManager> reportManager;
        private Mock<IReportValidationHub> reportValidationHub;

        private AcceptReportAssignmentCommand acceptReportAssignmentCommand;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new AcceptReportAssignmentRequest
            {
                ReportId = "xxx"
            };

            reportManager = new Mock<IReportManager>();
            reportValidationHub = new Mock<IReportValidationHub>();

            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(report);

            acceptReportAssignmentCommand =
                new AcceptReportAssignmentCommand(reportManager.Object, reportValidationHub.Object);
        }

        [Test]
        public void Handle_CurrentUserHasNoPermission_ThrowNoPermissionException()
        {
            reportValidationHub.Setup(rv =>
                    rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => acceptReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        [TestCase(false, false)]
        [TestCase(true, false)]
        public void Handle_AcceotReportAssignmentFailed_ThrowCrudException(bool isAccepted, bool isSucceeded)
        {
            SetUpReportAssignmentResult(isAccepted, isSucceeded);

            Assert.That(() => acceptReportAssignmentCommand.Handle(request, new CancellationToken()),
                Throws.TypeOf<CrudException>());
        }

        [Test]
        [TestCase(false, true)]
        [TestCase(true, true)]
        public async Task Handle_WhenCalled_ReturnAcceptReportAssignmentResponse(bool isAccepted, bool isSucceeded)
        {
            SetUpReportAssignmentResult(isAccepted, isSucceeded);

            var result = await acceptReportAssignmentCommand.Handle(request, new CancellationToken());

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<AcceptReportAssignmentResponse>());
            Assert.That(result.IsAccepted, Is.EqualTo(isAccepted));
            Assert.That(result.IsSucceeded, Is.EqualTo(isSucceeded));
        }

        #region private

        private void SetUpReportAssignmentResult(bool isAccepted, bool isSucceeded)
        {
           var reportAssignmentResult = new ReportAssignmentResult(isAccepted, isSucceeded);

            reportManager.Setup(rm => rm.AcceptReportAssignment(It.IsAny<Report>()))
                .ReturnsAsync(reportAssignmentResult);
        }

        #endregion
    }
}