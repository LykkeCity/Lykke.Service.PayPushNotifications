using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PayPushNotificationsSettings
    {
        public DbSettings Db { get; set; }

        public RabbitMqSubscriberSettings Rabbit { get; set; }

        public string HubName { get; set; }

        public string HubConnectionString { get; set; }
    }
}
