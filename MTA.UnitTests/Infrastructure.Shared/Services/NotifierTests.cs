using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;
using MTA.Infrastructure.Shared.Services;
using NUnit.Framework;

namespace MTA.UnitTests.Infrastructure.Shared.Services
{
    [TestFixture]
    public class NotifierTests
    {
        private Notifier notifier;

        private Mock<IDatabase> database;
        private Mock<INotifierValidationService> notifierValidationService;
        private Mock<IHttpContextReader> httpContextReader;

        private const string NotificationId = "id";

        [SetUp]
        public void SetUp()
        {
            var notification = new Notification();

            var notifications = new List<Notification>
            {
                new NotificationBuilder()
                    .SetText("text")
                    .SentTo(1)
                    .Build(),
                new NotificationBuilder()
                    .SetText("text")
                    .SentTo(2)
                    .Build(),
            };

            database = new Mock<IDatabase>();
            notifierValidationService = new Mock<INotifierValidationService>();
            httpContextReader = new Mock<IHttpContextReader>();

            database.Setup(d => d.NotificationRepository.Insert(It.IsNotNull<Notification>(), true))
                .ReturnsAsync(true);
            database.Setup(d => d.NotificationRepository.FindById(It.IsNotNull<string>()))
                .ReturnsAsync(notification);
            database.Setup(d => d.NotificationRepository.GetUserNotifications(It.IsNotNull<int>()))
                .ReturnsAsync(notifications);

            notifierValidationService.Setup(nvs => nvs.ValidateUserPermissions(It.IsNotNull<Notification>()))
                .Returns(true);

            notifier = new Notifier(database.Object, notifierValidationService.Object, httpContextReader.Object);
        }

        #region Push

        [Test]
        public async Task Push_WhenCalled_ReturnNotification()
        {
            var result = await notifier.Push(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>());

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Notification>());
        }

        [Test]
        public async Task Push_InsertingToDatabaseFailed_ReturnNull()
        {
            database.Setup(d => d.NotificationRepository.Insert(It.IsNotNull<Notification>(), true))
                .ReturnsAsync(false);

            var result = await notifier.Push(It.IsAny<string>(), It.IsAny<int>());

            Assert.That(result, Is.Null);
        }

        #endregion

        #region MarkAsRead

        [Test]
        public void MarkAsRead_NotificationNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.NotificationRepository.FindById(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => notifier.MarkAsRead(NotificationId), Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void MarkAsRead_ValidateUserPermissionsFailed_ThrowNoPermissionsException()
        {
            notifierValidationService.Setup(nvs => nvs.ValidateUserPermissions(It.IsNotNull<Notification>()))
                .Returns(false);

            Assert.That(() => notifier.MarkAsRead(NotificationId), Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task MarkAsRead_WhenCalled_ReturnIfRead(bool updated, bool expected)
        {
            database.Setup(d => d.NotificationRepository.Update(It.IsNotNull<Notification>()))
                .ReturnsAsync(updated);

            var result = await notifier.MarkAsRead(NotificationId);

            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region MarkAllAsRead

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task MarkAllAsRead_WhenCalled_ReturnIfRead(bool updated, bool expected)
        {
            database.Setup(d => d.NotificationRepository.UpdateRange(It.IsNotNull<IEnumerable<Notification>>()))
                .ReturnsAsync(updated);

            var result = await notifier.MarkAllAsRead();

            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion

        #region RemoveNotification

        [Test]
        public void RemoveNotification_NotificationNotFound_ThrowEntityNotFoundException()
        {
            database.Setup(d => d.NotificationRepository.FindById(It.IsNotNull<string>()))
                .ReturnsAsync(() => null);

            Assert.That(() => notifier.RemoveNotification(NotificationId),
                Throws.Exception.TypeOf<EntityNotFoundException>());
        }

        [Test]
        public void RemoveNotification_ValidateUserPermissionsFailed_ThrowNoPermissionsException()
        {
            notifierValidationService.Setup(nvs => nvs.ValidateUserPermissions(It.IsNotNull<Notification>()))
                .Returns(false);

            Assert.That(() => notifier.RemoveNotification(NotificationId),
                Throws.Exception.TypeOf<NoPermissionsException>());
        }

        [Test]
        [TestCase(true, true)]
        [TestCase(false, false)]
        public async Task RemoveNotification_WhenCalled_ReturnIfDeleted(bool deleted, bool expected)
        {
            database.Setup(d => d.NotificationRepository.Delete(It.IsNotNull<Notification>()))
                .ReturnsAsync(deleted);

            var result = await notifier.RemoveNotification(NotificationId);

            Assert.That(result, Is.EqualTo(expected));
        }

        #endregion
    }
}