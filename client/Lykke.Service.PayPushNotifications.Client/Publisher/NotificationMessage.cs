using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public class NotificationMessage: INotificationMessage
    {
        [PartitionOrRowKeys]
        public string[] MerchantIds { get; set; }

        [PartitionOrRowKeys]
        public string[] EmployeeIds { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
