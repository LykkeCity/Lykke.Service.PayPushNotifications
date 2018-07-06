using AzureStorage;
using Lykke.Service.PayPushNotifications.Core.Domain;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.AzureRepositories.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly INoSQLTableStorage<NotificationEntity> _storage;

        public NotificationRepository(INoSQLTableStorage<NotificationEntity> storage)
        {
            _storage = storage;
        }

        public Task InsertOrReplaceAsync(INotification notification)
        {
            return _storage.InsertOrReplaceAsync(new NotificationEntity(notification));
        }
    }
}
