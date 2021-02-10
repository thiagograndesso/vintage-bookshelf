using System.Collections.Generic;

namespace VintageBookshelf.Domain.Notifications
{
    public interface INotifier
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notification);
    }
}