namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public interface INotificationMessage
    {
        string[] MerchantIds { get; }

        string[] Emails { get; }
        
        string Message { get; }
    }
}
