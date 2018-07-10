using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }

        [AzureTableCheck]
        public string DataConnString { get; set; }

        public string EmployeeNotificationIdsTableName { get; set; }

        public string MerchantNotificationIdsTableName { get; set; }

        public string NotificationsTableName { get; set; }
    }
}
