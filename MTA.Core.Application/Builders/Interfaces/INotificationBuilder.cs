using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders.Interfaces
{
    public interface INotificationBuilder : IBuilder<Notification>
    {
        INotificationBuilder SetText(string text);
        INotificationBuilder SentTo(int userId);
        INotificationBuilder SetDetails(string details);
    }
}