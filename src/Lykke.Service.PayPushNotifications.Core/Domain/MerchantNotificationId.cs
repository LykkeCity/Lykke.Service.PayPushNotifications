namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public class MerchantNotificationId : IMerchantNotificationId
    {
        public string MerchantId { get; set; }
        public NotificationPlatform Platform { get; set; }
        public string NotificationId { get; set; }
    }
}
