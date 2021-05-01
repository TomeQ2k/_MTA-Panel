using System.Collections.Generic;
using System.Threading.Tasks;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Services.ReadOnly
{
    public interface IReadOnlyNotifier
    {
        Task<IEnumerable<Notification>> GetNotifications();

        Task<int> CountUnreadNotifications();
    }
}