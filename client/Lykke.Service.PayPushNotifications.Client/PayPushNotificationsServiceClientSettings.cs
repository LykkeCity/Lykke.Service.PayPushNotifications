using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.PayPushNotifications.Client 
{
    /// <summary>
    /// PayPushNotifications client settings.
    /// </summary>
    public class PayPushNotificationsServiceClientSettings 
    {
        /// <summary>Service url.</summary>
        [HttpCheck("api/isalive")]
        public string ServiceUrl {get; set;}
    }
}
