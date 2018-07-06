namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface IEmployeeNotificationId
    {
        string EmployeeEmail { get; }
        NotificationPlatform Platform { get; }
        string NotificationId { get; }
    }
}
