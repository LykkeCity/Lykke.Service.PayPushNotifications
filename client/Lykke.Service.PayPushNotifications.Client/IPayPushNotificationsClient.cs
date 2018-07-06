using JetBrains.Annotations;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Client
{
    /// <summary>
    /// PayPushNotifications client interface.
    /// </summary>
    [PublicAPI]
    public interface IPayPushNotificationsClient
    {
        #region NotificationController

        /// <summary>
        /// Get or create notification identities for mobile clients.
        /// </summary>
        /// <param name="employeeId">Email of the employee.</param>
        /// <param name="merchantId">Identy of the merchant.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>Notification identities.</returns>
        [Post("/api/Notification/GetNotificationIds/")]
        Task<Dictionary<string, string[]>> GetNotificationIdsAsync(string employeeId, string merchantId,
            CancellationToken cancellationToken = default(CancellationToken));

        #endregion NotificationController
    }
}
