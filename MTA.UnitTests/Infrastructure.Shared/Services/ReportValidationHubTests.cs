using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Common.Enums.Permissions;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportValidationHubTests
    {
        private ReportValidationHub reportValidationHub;

        private Mock<IReportValidationService> reportValidationService;
        private Mock<IReadOnlyReportService> reportService;
        private Mock<IHttpContextReader> httpContextReader;

        private Report report;

        private const int UserId = 1;
        private const string ReportId = "id";

        [SetUp]
        public void SetUp()
        {
            report = new Report();

            reportValidationService = new Mock<IReportValidationService>();
            reportService = new Mock<IReadOnlyReportService>();
            httpContextReader = new Mock<IHttpContextReader>();

            reportValidationService.Setup(rvs => rvs.IsUserReportMember(UserId, report))
                .ReturnsAsync(true);
            reportValidationService
                .Setup(rvs => rvs.ValidatePermissions(UserId, report, It.IsNotNull<ReportPermission[]>()))
                .ReturnsAsync(true);
            reportService.Setup(rs => rs.GetReport(It.IsNotNull<string>()))
                .ReturnsAsync(report);
            httpContextReader.Setup(h => h.CurrentUserId).Returns(UserId);

            reportValidationHub = new ReportValidationHub(reportValidationService.Object, reportService.Object,
                httpContextReader.Object);
        }

        #region ValidateAndReturnReport

        [Test]
        public void ValidateAndReturnReport_ReportNotFound_ThrowEntityNotFoundException()
        {
            reportService.Setup(rs => rs.GetReport(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => reportValidationHub.ValidateAndReturnReport(ReportId, ReportPermission.AddComment),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void ValidateAndReturnReport_UserIsNotMember_ThrowNoPermissionsException()
        {
            reportValidationService.Setup(rvs => rvs.IsUserReportMember(UserId, report))
                .ReturnsAsync(false);

            Assert.That(() => reportValidationHub.ValidateAndReturnReport(ReportId, ReportPermission.AddComment),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void ValidateAndReturnReport_UserIsNotValidated_ThrowNoPermissionsException()
        {
            reportValidationService
                .Setup(rvs => rvs.ValidatePermissions(UserId, report, It.IsNotNull<ReportPermission[]>()))
                .ReturnsAsync(false);

            Assert.That(() => reportValidationHub.ValidateAndReturnReport(ReportId, ReportPermission.AddComment),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public async Task ValidateAndReturnReport_UserIsValidated_ReturnReport()
        {
            var result =
                await reportValidationHub.ValidateAndReturnReport(ReportId, ReportPermission.AddComment);

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<Report>());
        }

        #endregion
    }
}