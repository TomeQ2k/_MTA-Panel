using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Results;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportManagerTests
    {
        private ReportManager reportManager;

        private Mock<IDatabase> database;

        private Report report;

        private const int UserId = 1;

        [SetUp]
        public void SetUp()
        {
            report = new Report();
            report.ChangeStatus(ReportStatusType.Closed);

            database = new Mock<IDatabase>();

            database.Setup(d => d.ReportRepository.Update(It.IsAny<Report>())).ReturnsAsync(true);
            database.Setup(d => d.ReportRepository.GetWhere(It.IsAny<string>()))
                .ReturnsAsync(new Collection<Report>());
            database.Setup(d => d.ReportRepository.UpdateRange(It.IsAny<List<Report>>())).ReturnsAsync(true);

            reportManager = new ReportManager(database.Object);
        }

        #region ChangeStatus

        [Test]
        public void ChangeStatus_NullReport_ThrowEntityNotFoundException()
        {
            Assert.That(() => reportManager.ChangeStatus(ReportStatusType.Awaiting, null),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task ChangeStatus_UpdatingReportFailed_ReturnFalse()
        {
            database.Setup(d => d.ReportRepository.Update(It.IsAny<Report>())).ReturnsAsync(false);
            var result = await reportManager.ChangeStatus(ReportStatusType.Awaiting, new Report());

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ChangeStatus_WhenCalled_ReturnTrue()
        {
            var result = await reportManager.ChangeStatus(ReportStatusType.Awaiting, new Report());

            Assert.That(result, Is.True);
        }

        #endregion

        #region AcceptReportAssignment

        [Test]
        public void AcceptReportAssignment_ReportNotFound_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportManager.AcceptReportAssignment(report),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task AcceptReportAssignment_WhenCalled_ReturnReportAssignmentResultWithIsAcceptedEqualTrue()
        {
            var result = await reportManager.AcceptReportAssignment(report);

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<ReportAssignmentResult>());
            Assert.That(result.IsAccepted, Is.True);
        }

        #endregion

        #region RejectReportAssignment

        [Test]
        public void RejectReportAssignment_ReportNotFound_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportManager.RejectReportAssignment(report),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task RejectReportAssignment_WhenCalled_ReturnReportAssignmentResultWithIsAcceptedEqualFalse()
        {
            var result = await reportManager.RejectReportAssignment(report);

            Assert.That(result, Is.Not.Null
                .And
                .TypeOf<ReportAssignmentResult>());
            Assert.That(result.IsAccepted, Is.False);
        }

        #endregion

        #region CloseReport

        [Test]
        public void CloseReport_ReportNotFound_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportManager.CloseReport(report),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CloseReport_ReportIsAlreadyClosed_ThrowNoPermissionsException()
        {
            report.ChangeStatus(ReportStatusType.Closed);

            Assert.That(() => reportManager.CloseReport(report),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public async Task CloseReport_WhenCalled_ReturnIsSucceeded()
        {
            report.ChangeStatus(ReportStatusType.Opened);

            var result = await reportManager.CloseReport(report);

            Assert.That(result, Is.True);
        }

        #endregion

        #region MoveReportAssignment

        [Test]
        public void MoveReportAssignment_ReportNotFound_ThrowEntityNotFoundException()
        {
            Assert.That(() => reportManager.MoveReportAssignment(null, UserId),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task MoveReportAssignment_WhenCalled_ReturnIsSucceeded()
        {
            var result = await reportManager.MoveReportAssignment(report, UserId);

            Assert.That(result, Is.True);
        }

        #endregion

        #region ArchiveReport

        [Test]
        public void ArchiveReport_NullReport_ThrowEntityNotFoundException()
        {
            Assert.That(() => reportManager.ArchiveReport(null),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void ArchiveReport_ReportNotClosed_NoPermissionsException()
        {
            report.ChangeStatus(ReportStatusType.Awaiting);

            Assert.That(() => reportManager.ArchiveReport(report),
                Throws.TypeOf<NoPermissionsException>());
        }

        [Test]
        public async Task ArchiveReport_UpdatingReportFailed_ReturnFalse()
        {
            database.Setup(d => d.ReportRepository.Update(It.IsAny<Report>())).ReturnsAsync(false);
            var result = await reportManager.ArchiveReport(report);

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task ArchiveReport_WhenCalled_ReturnTrue()
        {
            var result = await reportManager.ArchiveReport(report);

            Assert.That(result, Is.True);
        }

        #endregion

        #region ArchiveReports

        [Test]
        public async Task ArchiveReports_UpdatingReportsFailed_ReturnFalse()
        {
            database.Setup(d => d.ReportRepository.UpdateRange(It.IsAny<List<Report>>())).ReturnsAsync(false);
            var result = await reportManager.ArchiveReports();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.False);
        }

        [Test]
        public async Task ArchiveReports_WhenCalled_ReturnTrue()
        {
            var result = await reportManager.ArchiveReports();

            Assert.That(result, Is.Not.Null);
            Assert.That(result.IsSucceeded, Is.True);
        }

        #endregion

        #region TogglePrivacyReport

        [Test]
        public void TogglePrivacyReport_ReportNotFound_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportManager.TogglePrivacyReport(report),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task TogglePrivacyReport_WhenCalled_ReturnTogglePrivacyReportResult()
        {
            var result = await reportManager.TogglePrivacyReport(report);

            Assert.That(result, Is.TypeOf<TogglePrivacyReportResult>());
        }

        #endregion
    }
}