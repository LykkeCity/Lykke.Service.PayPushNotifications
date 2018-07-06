using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Common.Api.Contract.Responses;

namespace Lykke.Service.PayPushNotifications.Client
{
    /// <summary>
    /// PayPushNotifications client interface.
    /// </summary>
    [PublicAPI]
    public interface IPayPushNotificationsClient
    {
        #region IsAliveController

        /// <summary>
        /// Checks service is alive
        /// </summary>
        IsAliveResponse IsAlive();

        /// <summary>
        /// Checks service is alive
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        Task<IsAliveResponse> IsAliveAsync(CancellationToken cancellationToken = default(CancellationToken));

        #endregion IsAliveController

        #region NotificationController

        /// <summary>
        /// Get or create notification identities for mobile clients.
        /// </summary>
        /// <param name="employeeEmail">Email of the employee.</param>
        /// <param name="merchantId">Identy of the merchant.</param>
        /// <returns>Notification identities.</returns>
        Dictionary<string, string[]> GetNotificationIds(string employeeEmail, string merchantId);

        /// <summary>
        /// Get or create notification identities for mobile clients.
        /// </summary>
        /// <param name="employeeEmail">Email of the employee.</param>
        /// <param name="merchantId">Identy of the merchant.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>Notification identities.</returns>        
        Task<Dictionary<string, string[]>> GetNotificationIdsAsync(string employeeEmail, string merchantId,
            CancellationToken cancellationToken = default(CancellationToken));

        #endregion NotificationController
    }
}
