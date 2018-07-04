using JetBrains.Annotations;
using Lykke.Sdk.Settings;

namespace Lykke.Service.PayPushNotifications.Settings
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class AppSettings : BaseAppSettings
    {
        public PayPushNotificationsSettings PayPushNotificationsService { get; set; }        
    }
}
