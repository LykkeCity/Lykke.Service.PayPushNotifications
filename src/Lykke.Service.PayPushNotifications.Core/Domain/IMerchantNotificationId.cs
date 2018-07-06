namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface IMerchantNotificationId
    {
        string MerchantId { get; }
        NotificationPlatform Platform { get; }
        string NotificationId { get; }
    }
}
