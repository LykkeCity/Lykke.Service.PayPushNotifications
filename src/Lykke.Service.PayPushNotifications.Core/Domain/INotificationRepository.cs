using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface INotificationRepository
    {
        Task InsertOrReplaceAsync(INotification notification);
    }
}
