using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Core.Services
{
    public interface INotificationClient
    {
        Task SendNotificationAsync(string payLoad, IEnumerable<string> tags);
    }
}
