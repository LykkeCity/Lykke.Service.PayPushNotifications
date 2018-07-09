namespace Lykke.Service.PayPushNotifications.Core.Settings
{
    public interface IRabbitMqSubscriberSettings
    {
        string ConnectionString { get; }

        string ExchangeName { get; }

        string QueueName { get; }
    }
}
