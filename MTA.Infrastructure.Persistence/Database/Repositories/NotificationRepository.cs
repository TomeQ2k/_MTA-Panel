using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Application.Builders;
using MTA.Core.Common.Enums;
using MTA.Core.Domain.Data;
using MTA.Core.Domain.Data.Repositories;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Persistence.Database.Repositories
{
    public class NotificationRepository : Repository<Notification>, INotificationRepository
    {
        public NotificationRepository(ISqlConnectionFactory connectionFactory, string table) : base(connectionFactory,
            table)
        {
        }

        public async Task<IEnumerable<Notification>> GetUserNotifications(int userId)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("userId").Equals.Append(userId)
                .OrderBy("dateCreated", OrderByType.Descending)
                .Build());

        public async Task<IEnumerable<Notification>> GetUserUnreadNotifications(int userId)
            => await Query(new SqlBuilder()
                .Select()
                .From(Table)
                .Where("userId").Equals.Append(userId)
                .And.Append("isRead").Equals.False
                .OrderBy("dateCreated", OrderByType.Descending)
                .Build());
    }
}