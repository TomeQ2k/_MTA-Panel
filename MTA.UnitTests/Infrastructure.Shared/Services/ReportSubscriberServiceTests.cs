using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class ReportSubscriberServiceTests
    {
        private ReportSubscriberService reportSubscriberService;

        private Mock<IDatabase> database;

        private Report report;

        private const int CreatorId = 1;
        private const int AssigneeId = 2;
        private const int UserId = 3;

        [SetUp]
        public void SetUp()
        {
            report = new ReportBuilder()
                .CreatedBy(CreatorId)
                .Build();
            report.AssigneTo(AssigneeId);

            database = new Mock<IDatabase>();

            database.Setup(d => d.ReportSubscriberRepository.Insert(It.IsNotNull<ReportSubscriber>(), false))
                .ReturnsAsync(true);

            reportSubscriberService = new ReportSubscriberService(database.Object);
        }

        #region AddSubscriber

        [Test]
        public void AddSubscriber_ReportIsNull_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportSubscriberService.AddSubscriber(report, UserId),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void AddSubscriber_UserIsCreator_ThrowDuplicateException()
        {
            Assert.That(() => reportSubscriberService.AddSubscriber(report, CreatorId),
                Throws.Exception.TypeOf<DuplicateException>());
        }

        [Test]
        public void AddSubscriber_UserIsAssignee_ThrowDuplicateException()
        {
            Assert.That(() => reportSubscriberService.AddSubscriber(report, AssigneeId),
                Throws.Exception.TypeOf<DuplicateException>());
        }

        [Test]
        public void AddSubscriber_UserIsAlreadySubscriber_ThrowDuplicateException()
        {
            database.Setup(d => d.ReportSubscriberRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(new ReportSubscriber());

            Assert.That(() => reportSubscriberService.AddSubscriber(report, UserId),
                Throws.Exception.TypeOf<DuplicateException>());
        }

        [Test]
        public void AddSubscriber_InsertingSubscriberFailed_ThrowDatabaseException()
        {
            database.Setup(d => d.ReportSubscriberRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);
            database.Setup(d => d.ReportSubscriberRepository.Insert(It.IsNotNull<ReportSubscriber>(), false))
                .ReturnsAsync(false);

            Assert.That(() => reportSubscriberService.AddSubscriber(report, UserId),
                Throws.Exception.TypeOf<DatabaseException>());
        }

        [Test]
        public async Task AddSubscriber_WhenCalled_ReturnReportSubscriber()
        {
            database.Setup(d => d.ReportSubscriberRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);

            var result = await reportSubscriberService.AddSubscriber(report, UserId);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<ReportSubscriber>());
            Assert.That(result.ReportId, Is.EqualTo(report.Id));
            Assert.That(result.UserId, Is.EqualTo(3));
        }

        #endregion

        #region RemoveSubscriber

        [Test]
        public void RemoveSubscriber_ReportIsNull_ThrowEntityNotFoundException()
        {
            report = null;

            Assert.That(() => reportSubscriberService.RemoveSubscriber(report, UserId),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void RemoveSubscriber_UserIsCreator_ThrowNoPermissionsException()
        {
            Assert.That(() => reportSubscriberService.RemoveSubscriber(report, CreatorId),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void RemoveSubscriber_UserIsAssignee_ThrowNoPermissionsException()
        {
            Assert.That(() => reportSubscriberService.RemoveSubscriber(report, AssigneeId),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        public void RemoveSubscriber_UserIsNotSubscriber_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.ReportSubscriberRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => reportSubscriberService.RemoveSubscriber(report, UserId),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public async Task RemoveSubscriber_WhenCalled_ReturnIsDeleted()
        {
            database.Setup(d => d.ReportSubscriberRepository.Find(It.IsNotNull<string>()))
                .ReturnsAsync(new ReportSubscriber());

            var result = await reportSubscriberService.RemoveSubscriber(report, UserId);

            Assert.That(result, Is.TypeOf<bool>());
        }

        #endregion
    }
}