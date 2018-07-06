using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public class RabbitMqPublisherSettings
    {
        [AmqpCheck]
        public string ConnectionString { get; set; }

        public string ExchangeName { get; set; }
    }
}
