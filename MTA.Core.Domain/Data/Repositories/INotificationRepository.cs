using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Domain.Data.Repositories
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task<IEnumerable<Notification>> GetUserNotifications(int userId);
        Task<IEnumerable<Notification>> GetUserUnreadNotifications(int userId);
    }
}