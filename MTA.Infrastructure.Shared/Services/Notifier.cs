using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Application.Exceptions;
using MTA.Core.Application.Services;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class Notifier : INotifier
    {
        private readonly IDatabase database;
        private readonly INotifierValidationService notifierValidationService;
        private readonly IHttpContextReader httpContextReader;

        public Notifier(IDatabase database, INotifierValidationService notifierValidationService,
            IHttpContextReader httpContextReader)
        {
            this.database = database;
            this.notifierValidationService = notifierValidationService;
            this.httpContextReader = httpContextReader;
        }

        public async Task<IEnumerable<Notification>> GetNotifications()
            => await database.NotificationRepository.GetUserNotifications(httpContextReader.CurrentUserId);

        public async Task<Notification> Push(string text, int userId, string details = null)
        {
            var notification = new NotificationBuilder()
                .SetText(text)
                .SentTo(userId)
                .SetDetails(details)
                .Build();

            return await database.NotificationRepository.Insert(notification) ? notification : null;
        }

        public async Task<IEnumerable<Notification>> PushToGroup(string text, IEnumerable<int> usersIds,
            string details = null)
        {
            IList<Notification> notifications = new List<Notification>();

            foreach (var userId in usersIds)
                notifications.Add(await Push(text, userId, details));

            return notifications;
        }

        public async Task<bool> MarkAsRead(string notificationId)
        {
            var notification = await database.NotificationRepository.FindById(notificationId)
                               ?? throw new EntityNotFoundException("Notification not found");

            if (!notifierValidationService.ValidateUserPermissions(notification))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            notification.MarkAsRead();

            return await database.NotificationRepository.Update(notification);
        }

        public async Task<bool> MarkAllAsRead()
        {
            var userNotifications = await GetNotifications();

            userNotifications.ToList().ForEach(n => n.MarkAsRead());

            return await database.NotificationRepository.UpdateRange(userNotifications);
        }

        public async Task<bool> RemoveNotification(string notificationId)
        {
            var notification = await database.NotificationRepository.FindById(notificationId)
                               ?? throw new EntityNotFoundException("Notification not found");

            if (!notifierValidationService.ValidateUserPermissions(notification))
                throw new NoPermissionsException(ErrorMessages.NotAllowedMessage);

            return await database.NotificationRepository.Delete(notification);
        }

        public async Task<bool> ClearAllNotifications()
        {
            var notificationsToDelete = (await GetNotifications()).Select(n => new ColumnValue("id", n.Id));

            return await database.NotificationRepository.DeleteRangeByColumn(notificationsToDelete.ToArray());
        }

        public async Task<int> CountUnreadNotifications()
            => (await database.NotificationRepository.GetUserUnreadNotifications(httpContextReader.CurrentUserId))
                .Count();
    }
}