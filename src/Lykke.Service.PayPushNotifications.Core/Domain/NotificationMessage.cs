namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public class NotificationMessage
    {
        public string[] MerchantIds { get; set; }

        public string[] EmployeeIds { get; set; }

        public string Message { get; set; }
    }
}
