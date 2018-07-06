namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public class EmployeeNotificationId: IEmployeeNotificationId
    {
        public string EmployeeId { get; set; }
        public NotificationPlatform Platform { get; set; }
        public string NotificationId { get; set; }
    }
}
