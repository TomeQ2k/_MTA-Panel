using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Features.Requests.Commands;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Persistence.Database;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportServiceTests
    {
        private CreateOtherReportRequest createOtherReportRequest;
        private CreateUserReportRequest createUserReportRequest;
        private CreatePenaltyReportRequest createPenaltyReportRequest;
        private CreateBugReportRequest createBugReportRequest;
        private User creator;

        private Mock<IDatabase> database;
        private Mock<IReportImageService> reportImageService;
        private Mock<IHttpContextReader> httpContextReader;
        private ReportService reportService;

        [SetUp]
        public void SetUp()
        {
            createOtherReportRequest = new CreateOtherReportRequest();
            createUserReportRequest = new CreateUserReportRequest
            {
                UserToReportId = 1
            };
            createPenaltyReportRequest = new CreatePenaltyReportRequest();
            createBugReportRequest = new CreateBugReportRequest();

            creator = new User();

            database = new Mock<IDatabase>();
            reportImageService = new Mock<IReportImageService>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(creator);
            database.Setup(d => d.ReportRepository.Insert(It.IsAny<Report>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.BugReportRepository.Insert(It.IsAny<BugReport>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.PenaltyReportRepository.Insert(It.IsAny<PenaltyReport>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.UserReportRepository.Insert(It.IsAny<UserReport>(), false))
                .ReturnsAsync(true);
            database.Setup(d => d.BeginTransaction()).Returns(new DatabaseTransaction());

            reportService = new ReportService(database.Object, reportImageService.Object, httpContextReader.Object);
        }


        #region CreateOtherReport

        [Test]
        public void CreateOtherReport_CreatingBaseReportFailedUserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => reportService.CreateOtherReport(createOtherReportRequest),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CreateOtherReport_InsertReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportRepository.Insert(It.IsAny<Report>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreateOtherReport(createOtherReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateOtherReport_WhenCalled_ReturnReport()
        {
            var result = await reportService.CreateOtherReport(createOtherReportRequest);

            Assert.That(result, Is.TypeOf<Report>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.CreatorId, Is.EqualTo(creator.Id));
        }

        #endregion

        #region CreateUserReport

        [Test]
        public void CreateUserReport_CreatingBaseReportFailedUserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => reportService.CreateUserReport(createUserReportRequest),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CreateUserReport_CreatingBaseReportFailedUserNotFound1_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(creator.Id)).ReturnsAsync(creator);
            database.Setup(d => d.UserRepository.FindById(createUserReportRequest.UserToReportId))
                .ReturnsAsync(() => null);


            Assert.That(() => reportService.CreateUserReport(createUserReportRequest),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CreateUserReport_InsertReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportRepository.Insert(It.IsAny<Report>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreateUserReport(createUserReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void CreateUserReport_InsertUserReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.UserReportRepository.Insert(It.IsAny<UserReport>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreateUserReport(createUserReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateUserReport_WhenCalled_ReturnReport()
        {
            var result = await reportService.CreateUserReport(createUserReportRequest);

            Assert.That(result, Is.TypeOf<UserReport>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Report.CreatorId, Is.EqualTo(creator.Id));
        }

        #endregion

        #region CreatePenaltyReport

        [Test]
        public void CreatePenaltyReport_CreatingBaseReportFailedUserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => reportService.CreatePenaltyReport(createPenaltyReportRequest),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CreatePenaltyReport_InsertReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportRepository.Insert(It.IsAny<Report>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreatePenaltyReport(createPenaltyReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void CreatePenaltyReport_InsertUserReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.PenaltyReportRepository.Insert(It.IsAny<PenaltyReport>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreatePenaltyReport(createPenaltyReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreatePenaltyReport_WhenCalled_ReturnReport()
        {
            var result = await reportService.CreatePenaltyReport(createPenaltyReportRequest);

            Assert.That(result, Is.TypeOf<PenaltyReport>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Report.CreatorId, Is.EqualTo(creator.Id));
        }

        #endregion

        #region CreateBugReport

        [Test]
        public void CreateBugReport_CreatingBaseReportFailedUserNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.UserRepository.FindById(It.IsAny<int>())).ReturnsAsync(() => null);

            Assert.That(() => reportService.CreateBugReport(createBugReportRequest),
                Throws.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void CreateBugReport_InsertReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportRepository.Insert(It.IsAny<Report>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreateBugReport(createBugReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public void CreateBugReport_InsertUserReportFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.BugReportRepository.Insert(It.IsAny<BugReport>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportService.CreateBugReport(createBugReportRequest),
                Throws.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task CreateBugReport_WhenCalled_ReturnReport()
        {
            var result = await reportService.CreateBugReport(createBugReportRequest);

            Assert.That(result, Is.TypeOf<BugReport>());
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Report.CreatorId, Is.EqualTo(creator.Id));
        }

        #endregion
    }
}