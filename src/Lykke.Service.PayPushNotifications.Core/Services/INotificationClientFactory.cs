using Lykke.Service.PayPushNotifications.Core.Domain;

namespace Lykke.Service.PayPushNotifications.Core.Services
{
    public interface INotificationClientFactory
    {
        INotificationClient GetClient(NotificationPlatform notificationPlatform);
    }
}
