using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Core.Domain
{
    public interface IEmployeeNotificationIdRepository
    {
        Task<IEnumerable<IEmployeeNotificationId>> GetAsync(string EmployeeId);
        Task<IEnumerable<IEmployeeNotificationId>> GetAsync(IEnumerable<string> EmployeeId);
        Task InsertOrReplaceAsync(IEmployeeNotificationId employeeNotificationId);
        Task InsertOrReplaceBatchAsync(IEnumerable<IEmployeeNotificationId> employeeNotificationIds);
    }
}
