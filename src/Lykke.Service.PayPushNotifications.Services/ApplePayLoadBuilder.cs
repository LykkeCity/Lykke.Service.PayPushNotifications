using Common;
using System.Collections.Generic;
using Lykke.Service.PayPushNotifications.Core.Services;

namespace Lykke.Service.PayPushNotifications.Services
{
    public class ApplePayLoadBuilder : IPayLoadBuilder
    {
        private readonly Dictionary<string, object> _dictionary;
        private readonly Dictionary<string, object> _aps;

        public ApplePayLoadBuilder()
        {
            _aps = new Dictionary<string, object>();
            _dictionary = new Dictionary<string, object> {{AppleApsKeys.Aps, _aps}};
        }

        public void AddMessage(string message)
        {
            _aps[AppleApsKeys.Alert] = message;
        }

        public override string ToString()
        {
            return _dictionary.ToJson();
        }
    }
}
