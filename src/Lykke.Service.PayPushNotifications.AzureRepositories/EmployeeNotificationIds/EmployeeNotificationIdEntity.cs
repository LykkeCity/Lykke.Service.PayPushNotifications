using Lykke.AzureStorage.Tables;
using Lykke.AzureStorage.Tables.Entity.Annotation;
using Lykke.AzureStorage.Tables.Entity.ValueTypesMerging;
using Lykke.Service.PayPushNotifications.Core.Domain;
using System;

namespace Lykke.Service.PayPushNotifications.AzureRepositories.EmployeeNotificationIds
{
    [ValueTypeMergingStrategy(ValueTypeMergingStrategy.UpdateIfDirty)]
    public class EmployeeNotificationIdEntity : AzureTableEntity, IEmployeeNotificationId
    {
        public string EmployeeEmail => PartitionKey;

        public NotificationPlatform Platform => Enum.Parse<NotificationPlatform>(RowKey, true);

        public string NotificationId { get; set; }

        public EmployeeNotificationIdEntity()
        {
        }

        public EmployeeNotificationIdEntity(IEmployeeNotificationId employeeNotificationId)
        {
            PartitionKey = GetPartitionKey(employeeNotificationId.EmployeeEmail);
            RowKey = GetRowKey(employeeNotificationId.Platform);
            NotificationId = employeeNotificationId.NotificationId;
        }

        internal static string GetPartitionKey(string employeeEmail)
            => employeeEmail;
        internal static string GetRowKey(NotificationPlatform platform)
            => platform.ToString();
    }
}
