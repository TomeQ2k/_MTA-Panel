using MTA.Core.Application.Builders.Interfaces;
using MTA.Core.Domain.Entities;

namespace MTA.Core.Application.Builders
{
    public class NotificationBuilder : INotificationBuilder
    {
        private readonly Notification notification = new Notification();

        public INotificationBuilder SetText(string text)
        {
            notification.SetText(text);
            return this;
        }

        public INotificationBuilder SentTo(int userId)
        {
            notification.SetUserId(userId);
            return this;
        }

        public INotificationBuilder SetDetails(string details)
        {
            notification.SetDetails(details);
            return this;
        }

        public Notification Build() => this.notification;
    }
}