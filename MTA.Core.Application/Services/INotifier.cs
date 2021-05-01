using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Services.ReadOnly;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services
{
    public interface INotifier : IReadOnlyNotifier
    {
        Task<Notification> Push(string text, int userId, string details = null);
        Task<IEnumerable<Notification>> PushToGroup(string text, IEnumerable<int> usersIds, string details = null);

        Task<bool> MarkAsRead(string notificationId);
        Task<bool> MarkAllAsRead();

        Task<bool> RemoveNotification(string notificationId);
        Task<bool> ClearAllNotifications();
    }
}