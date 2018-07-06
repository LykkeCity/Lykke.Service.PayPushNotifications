// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Lykke.Service.PayPushNotifications.Client.AutorestClient
{
    using Models;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension methods for PayPushNotificationsAPI.
    /// </summary>
    public static partial class PayPushNotificationsAPIExtensions
    {
            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IsAliveResponse IsAlive(this IPayPushNotificationsAPI operations)
            {
                return operations.IsAliveAsync().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Checks service is alive
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IsAliveResponse> IsAliveAsync(this IPayPushNotificationsAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.IsAliveWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get or create notification identities for mobile clients.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='employeeEmail'>
            /// Email of the employee.
            /// </param>
            /// <param name='merchantId'>
            /// Identy of the merchant.
            /// </param>
            public static object GetNotificationIds(this IPayPushNotificationsAPI operations, string employeeEmail = default(string), string merchantId = default(string))
            {
                return operations.GetNotificationIdsAsync(employeeEmail, merchantId).GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get or create notification identities for mobile clients.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='employeeEmail'>
            /// Email of the employee.
            /// </param>
            /// <param name='merchantId'>
            /// Identy of the merchant.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> GetNotificationIdsAsync(this IPayPushNotificationsAPI operations, string employeeEmail = default(string), string merchantId = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetNotificationIdsWithHttpMessagesAsync(employeeEmail, merchantId, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}