using Lykke.Common.Api.Contract.Responses;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Lykke.Service.PayPushNotifications.Client.Models
{
    [Serializable]
    public class PayPushNotificationsApiException : Exception
    {
        public IDictionary<string, List<string>> ModelErrors { get; }

        public PayPushNotificationsApiException()
        {
        }

        public PayPushNotificationsApiException(ErrorResponse errorResponse)
            : base(errorResponse.ErrorMessage)
        {
            ModelErrors = errorResponse.ModelErrors;
        }

        public PayPushNotificationsApiException(string message,
            IDictionary<string, List<string>> modelErrors = null)
            : base(message)
        {
            ModelErrors = modelErrors;
        }

        public PayPushNotificationsApiException(string message,
            Exception innerException,
            IDictionary<string, List<string>> modelErrors = null)
            : base(message, innerException)
        {
            ModelErrors = modelErrors;
        }

        protected PayPushNotificationsApiException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
