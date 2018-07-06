using System;

namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public class Notification : INotification
    {
        public string Id { get; }
        public DateTime Sent { get; }
        public NotificationMessage NotificationMessage { get; set; }

        public Notification()
        {
            Id = Guid.NewGuid().ToString();
            Sent = DateTime.UtcNow;
        }
    }
}
