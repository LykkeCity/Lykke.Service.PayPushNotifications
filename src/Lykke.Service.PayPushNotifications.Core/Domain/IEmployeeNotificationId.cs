namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface IEmployeeNotificationId
    {
        string EmployeeId { get; }
        NotificationPlatform Platform { get; }
        string NotificationId { get; }
    }
}
