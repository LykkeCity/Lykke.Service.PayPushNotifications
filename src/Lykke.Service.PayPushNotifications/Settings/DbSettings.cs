using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Settings
{
    public class DbSettings
    {
        [AzureTableCheck]
        public string LogsConnString { get; set; }
    }
}
