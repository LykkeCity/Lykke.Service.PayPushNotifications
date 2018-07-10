namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public interface INotificationMessage
    {
        string[] MerchantIds { get; }

        string[] EmployeeIds { get; }
        
        string Message { get; }
    }
}
