using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PayPushNotifications.Core.Domain;

namespace Lykke.Service.PayPushNotifications.AzureRepositories.MerchantNotificationIds
{
    public class MerchantNotificationIdRepository : IMerchantNotificationIdRepository
    {
        private readonly INoSQLTableStorage<MerchantNotificationIdEntity> _storage;

        public MerchantNotificationIdRepository(INoSQLTableStorage<MerchantNotificationIdEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IMerchantNotificationId>> GetAsync(string merchantId)
        {
            return await _storage.GetDataAsync(MerchantNotificationIdEntity.GetPartitionKey(merchantId));
        }

        public async Task<IEnumerable<IMerchantNotificationId>> GetAsync(IEnumerable<string> merchantIds)
        {
            return await _storage.GetDataAsync(merchantIds.Select(MerchantNotificationIdEntity.GetPartitionKey));
        }

        public Task InsertOrReplaceAsync(IMerchantNotificationId merchantNotificationId)
        {
            return _storage.InsertOrReplaceAsync(new MerchantNotificationIdEntity(merchantNotificationId));
        }

        public Task InsertOrReplaceBatchAsync(IEnumerable<IMerchantNotificationId> merchantNotificationIds)
        {
            return _storage.InsertOrReplaceBatchAsync(
                merchantNotificationIds.Select(i => new MerchantNotificationIdEntity(i)));
        }
    }
}
