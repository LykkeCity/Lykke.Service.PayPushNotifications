using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.PayPushNotifications.Core.Domain;

namespace Lykke.Service.PayPushNotifications.AzureRepositories.EmployeeNotificationIds
{
    public class EmployeeNotificationIdRepository : IEmployeeNotificationIdRepository
    {
        private readonly INoSQLTableStorage<EmployeeNotificationIdEntity> _storage;

        public EmployeeNotificationIdRepository(INoSQLTableStorage<EmployeeNotificationIdEntity> storage)
        {
            _storage = storage;
        }

        public async Task<IEnumerable<IEmployeeNotificationId>> GetAsync(string employeeEmail)
        {
            return await _storage.GetDataAsync(EmployeeNotificationIdEntity.GetPartitionKey(employeeEmail));
        }

        public async Task<IEnumerable<IEmployeeNotificationId>> GetAsync(IEnumerable<string> employeeEmails)
        {
            return await _storage.GetDataAsync(employeeEmails.Select(EmployeeNotificationIdEntity.GetPartitionKey));
        }

        public Task InsertOrReplaceAsync(IEmployeeNotificationId employeeNotificationId)
        {
            return _storage.InsertOrReplaceAsync(new EmployeeNotificationIdEntity(employeeNotificationId));
        }

        public Task InsertOrReplaceBatchAsync(IEnumerable<IEmployeeNotificationId> employeeNotificationIds)
        {
            return _storage.InsertOrReplaceBatchAsync(
                employeeNotificationIds.Select(i => new EmployeeNotificationIdEntity(i)));
        }
    }
}
