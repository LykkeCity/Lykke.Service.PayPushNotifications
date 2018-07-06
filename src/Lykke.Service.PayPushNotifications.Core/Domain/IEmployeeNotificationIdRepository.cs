using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface IEmployeeNotificationIdRepository
    {
        Task<IEnumerable<IEmployeeNotificationId>> GetAsync(string employeeEmail);
        Task<IEnumerable<IEmployeeNotificationId>> GetAsync(IEnumerable<string> employeeEmails);
        Task InsertOrReplaceAsync(IEmployeeNotificationId employeeNotificationId);
        Task InsertOrReplaceBatchAsync(IEnumerable<IEmployeeNotificationId> employeeNotificationIds);
    }
}
