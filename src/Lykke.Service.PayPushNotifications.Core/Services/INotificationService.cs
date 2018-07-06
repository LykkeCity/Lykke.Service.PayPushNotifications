using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.PayPushNotifications.Core.Domain;

namespace Lykke.Service.PayPushNotifications.Core.Services
{
    public interface INotificationService
    {
        Task SendAsync(NotificationMessage notification);

        Task<Dictionary<NotificationPlatform, string[]>> GetNotificationIdsAsync(string employeeEmail,
            string merchantId);
    }
}
