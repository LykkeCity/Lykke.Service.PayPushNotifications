using Lykke.Service.PayPushNotifications.Core.Settings;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Settings
{
    public class RabbitMqSubscriberSettings : IRabbitMqSubscriberSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string ExchangeName { get; set; }

        public string QueueName { get; set; }
    }
}
