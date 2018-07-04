using JetBrains.Annotations;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PayPushNotificationsSettings
    {
        public DbSettings Db { get; set; }
    }
}
