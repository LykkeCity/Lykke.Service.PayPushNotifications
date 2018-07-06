using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public class NotificationMessage: INotificationMessage
    {
        public string[] MerchantIds { get; set; }

        public string[] EmployeeIds { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
