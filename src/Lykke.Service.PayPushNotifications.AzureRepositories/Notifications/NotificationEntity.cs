using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.Service.PayPushNotifications.Core.Domain;
using System;

namespace Lykke.Service.PayPushNotifications.AzureRepositories.Notifications
{
    public class NotificationEntity : AzureTableEntity, INotification
    {
        public string Id => RowKey;

        public DateTime Sent { get; set; }

        [JsonValueSerializer]
        public NotificationMessage NotificationMessage { get; set; }

        public NotificationEntity()
        {
        }

        public NotificationEntity(INotification notification)
        {
            Sent = notification.Sent;
            PartitionKey = GetPartitionKey(Sent);
            RowKey = GetRowKey(notification.Id);

            NotificationMessage = notification.NotificationMessage;
        }

        internal static string GetPartitionKey(DateTime sent)
            => sent.ToString("yyyy-MM-ddTHH:mm");
        internal static string GetRowKey(string id)
            => id;
    }
}
