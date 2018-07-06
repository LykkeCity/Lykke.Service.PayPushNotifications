using Lykke.Service.PayPushNotifications.Core.Domain;
using Lykke.Service.PayPushNotifications.Core.Services;
using System;

namespace Lykke.Service.PayPushNotifications.Services
{
    public class PayLoadBuilderFactory : IPayLoadBuilderFactory
    {
        public IPayLoadBuilder CreateBuilder(NotificationPlatform notificationPlatform)
        {
            switch (notificationPlatform)
            {
                case NotificationPlatform.Aps: return new ApplePayLoadBuilder();
                default: throw new ArgumentOutOfRangeException(nameof(notificationPlatform));
            }
        }
    }
}
