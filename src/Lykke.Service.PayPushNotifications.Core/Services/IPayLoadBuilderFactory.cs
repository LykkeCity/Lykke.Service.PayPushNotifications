using Lykke.Service.PayPushNotifications.Core.Domain;

namespace Lykke.Service.PayPushNotifications.Core.Services
{
    public interface IPayLoadBuilderFactory
    {
        IPayLoadBuilder CreateBuilder(NotificationPlatform notificationPlatform);
    }
}
