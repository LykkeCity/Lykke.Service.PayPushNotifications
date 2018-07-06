using System;

namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface INotification
    {
        string Id { get; }
        DateTime Sent { get; }
        NotificationMessage NotificationMessage { get; }
    }
}
