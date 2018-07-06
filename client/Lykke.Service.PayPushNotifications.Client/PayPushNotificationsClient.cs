using Lykke.Common.Api.Contract.Responses;
using Lykke.Service.PayPushNotifications.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.PayPushNotifications.Client.AutorestClient;

namespace Lykke.Service.PayPushNotifications.Client
{
    /// <summary>
    /// PayPushNotifications client.
    /// </summary>
    public class PayPushNotificationsClient : IPayPushNotificationsClient, IDisposable
    {
        private readonly IPayPushNotificationsAPI _service;

        public PayPushNotificationsClient(string serviceUrl)
        {
            _service = new PayPushNotificationsAPI(new Uri(serviceUrl));
        }

        #region IsAliveController

        /// <summary>
        /// Checks service is alive
        /// </summary>
        public IsAliveResponse IsAlive()
        {
            object result = _service.IsAlive();
            return Convert<IsAliveResponse>(result);
        }

        /// <summary>
        /// Checks service is alive
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        public async Task<IsAliveResponse> IsAliveAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            object result = await _service.IsAliveAsync(cancellationToken);
            return Convert<IsAliveResponse>(result);
        }

        #endregion IsAliveController

        #region NotificationController

        /// <summary>
        /// Get or create notification identities for mobile clients.
        /// </summary>
        /// <param name="employeeEmail">Email of the employee.</param>
        /// <param name="merchantId">Identy of the merchant.</param>
        /// <returns>Notification identities.</returns>
        public Dictionary<string, string[]> GetNotificationIds(string employeeEmail, string merchantId)
        {
            var result = _service.GetNotificationIds(employeeEmail, merchantId);
            return Convert<Dictionary<string, string[]>>(result);
        }

        /// <summary>
        /// Get or create notification identities for mobile clients.
        /// </summary>
        /// <param name="employeeEmail">Email of the employee.</param>
        /// <param name="merchantId">Identy of the merchant.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work.</param>
        /// <returns>Notification identities.</returns>        
        public async Task<Dictionary<string, string[]>> GetNotificationIdsAsync(string employeeEmail, string merchantId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var result = await _service.GetNotificationIdsAsync(employeeEmail, merchantId, cancellationToken);
            return Convert<Dictionary<string, string[]>>(result);
        }

        #endregion NotificationController

        public void Dispose()
        {
            _service?.Dispose();
        }

        private static T Convert<T>(object result) where T : class
        {
            if (result is ErrorResponse errorResponse)
            {
                throw new PayPushNotificationsApiException(errorResponse);
            }

            return result as T;
        }
    }
}
