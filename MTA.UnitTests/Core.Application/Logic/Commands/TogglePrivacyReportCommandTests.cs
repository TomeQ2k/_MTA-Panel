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
    public class TogglePrivacyReportCommandTests
    {
        private TogglePrivacyReportCommand togglePrivacyReportCommand;

        private Mock<IReportManager> reportMananger;

        private Report report;
        private TogglePrivacyReportRequest request;
        private Mock<IReportValidationHub> reportValidationHub;


        [SetUp]
        public void SetUp()
        {
            report = new Report();
            request = new TogglePrivacyReportRequest();
            var togglePrivacyReportResult = new TogglePrivacyReportResult(true, true);

            reportMananger = new Mock<IReportManager>();
            reportValidationHub = new Mock<IReportValidationHub>();

            reportMananger.Setup(rm => rm.TogglePrivacyReport(It.IsAny<Report>()))
                .ReturnsAsync(togglePrivacyReportResult);
            reportValidationHub.Setup(rv =>
                rv.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>())).ReturnsAsync(report);

            togglePrivacyReportCommand =
                new TogglePrivacyReportCommand(reportMananger.Object, reportValidationHub.Object);
        }

        [Test]
        public void Handle_ReportValidationFailed_ThrowNoPermissionsException()
        {
            reportValidationHub.Setup(
                    rvh => rvh.ValidateAndReturnReport(It.IsAny<string>(), It.IsAny<ReportPermission[]>()))
                .ReturnsAsync(() => null);

            Assert.That(() => togglePrivacyReportCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void Handle_TogglingPrivacyReportFailed_ThrowCrudException()
        {
            reportMananger.Setup(rm => rm.TogglePrivacyReport(It.IsAny<Report>()))
                .ReturnsAsync(new TogglePrivacyReportResult(true, false));

            Assert.That(() => togglePrivacyReportCommand.Handle(request, It.IsNotNull<CancellationToken>()),
                Throws.Exception.TypeOf<CrudException>());
        }

        [Test]
        public async Task Handle_WhenCalled_ReturnTogglePrivacyReportResponse()
        {
            var result = await togglePrivacyReportCommand.Handle(request, It.IsNotNull<CancellationToken>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
            Assert.That(result, Is.TypeOf<TogglePrivacyReportResponse>());
        }
    }
}