using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface IMerchantNotificationIdRepository
    {
        Task<IEnumerable<IMerchantNotificationId>> GetAsync(string merchantId);
        Task<IEnumerable<IMerchantNotificationId>> GetAsync(IEnumerable<string> merchantId);
        Task InsertOrReplaceAsync(IMerchantNotificationId merchantNotificationId);
        Task InsertOrReplaceBatchAsync(IEnumerable<IMerchantNotificationId> merchantNotificationIds);
    }
}
