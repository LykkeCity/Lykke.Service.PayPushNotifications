using Lykke.Service.PayPushNotifications.Core.Domain;
using Lykke.Service.PayPushNotifications.Core.Services;
using System;
using System.Collections.Generic;

namespace Lykke.Service.PayPushNotifications.Services
{
    public class NotificationClientFactory : INotificationClientFactory
    {
        private readonly Dictionary<NotificationPlatform, INotificationClient> _clients;

        public NotificationClientFactory(Dictionary<NotificationPlatform, INotificationClient> clients)
        {
            _clients = clients;
        }

        public INotificationClient GetClient(NotificationPlatform notificationPlatform)
        {
            if (_clients.TryGetValue(notificationPlatform, out var client))
            {
                return client;
            }

            throw new ArgumentOutOfRangeException(nameof(notificationPlatform));
        }
    }
}
