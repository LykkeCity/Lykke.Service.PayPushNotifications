using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.PayPushNotifications.Core.Domain;
using System;

namespace Lykke.Service.PayPushNotifications.AzureRepositories.MerchantNotificationIds
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class MerchantNotificationIdEntity : AzureTableEntity, IMerchantNotificationId
    {
        public string MerchantId => PartitionKey;

        public NotificationPlatform Platform => Enum.Parse<NotificationPlatform>(RowKey, true);

        public string NotificationId { get; set; }

        public MerchantNotificationIdEntity()
        {
        }

        public MerchantNotificationIdEntity(IMerchantNotificationId merchantNotificationId)
        {
            PartitionKey = GetPartitionKey(merchantNotificationId.MerchantId);
            RowKey = GetRowKey(merchantNotificationId.Platform);
            NotificationId = merchantNotificationId.NotificationId;
        }

        internal static string GetPartitionKey(string merchantId)
            => merchantId;
        internal static string GetRowKey(NotificationPlatform platform)
            => platform.ToString();
    }
}
