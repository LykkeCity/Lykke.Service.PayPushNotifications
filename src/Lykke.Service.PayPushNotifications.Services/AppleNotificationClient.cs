using Lykke.Service.PayPushNotifications.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;

namespace Lykke.Service.PayPushNotifications.Services
{
    public class AppleNotificationClient : INotificationClient
    {
        private readonly string _sharedAccessKey;
        private readonly string _sharedAccessKeyName;
        private readonly string _url;
        private readonly ILog _log;

        public AppleNotificationClient(string sharedAccessKey, string sharedAccessKeyName, string baseUrl, 
            string hubName, ILogFactory logFactory)
        {
            _sharedAccessKey = sharedAccessKey;
            _sharedAccessKeyName = sharedAccessKeyName;
            _url = string.Format("https://{0}/{1}/messages/?api-version=2015-08", baseUrl, hubName);
            _log = logFactory.CreateLog(this);
        }

        public static AppleNotificationClient CreateClientFromConnectionString(string connectionString,
            string hubName, ILogFactory logFactory)
        {
            var regexp = new Regex(@"sb://(?<url>[A-z\.\-]*)/;SharedAccessKeyName=(?<keyName>[A-z0-9]*);.*SharedAccessKey=(?<key>[A-z0-9+=/]*)");
            var match = regexp.Match(connectionString);
            var baseUrl = match.Groups["url"].Value;
            var accessKey = match.Groups["key"].Value;
            var accessKeyName = match.Groups["keyName"].Value;

            return new AppleNotificationClient(accessKey, accessKeyName, baseUrl, hubName, logFactory);
        }

        public Task SendNotificationAsync(string payLoad, IEnumerable<string> tags)
        {
            var headers = new Dictionary<string, string>
            {
                {"ServiceBusNotification-Format", "apple"},
                {"ServiceBusNotification-Tags", string.Join("||", tags)},
                {"ServiceBusNotification-Apns-Expiry", DateTime.UtcNow.AddDays(10).ToString("s")}
            };

            return SendNotification(payLoad, headers);
        }

        public async Task SendNotification(string payload, Dictionary<string, string> headers)
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8";

            var epochTime = (long)(DateTime.UtcNow - new DateTime(1970, 01, 01)).TotalSeconds;
            var expiry = epochTime + (long)TimeSpan.FromHours(1).TotalSeconds;

            var encodedUrl = WebUtility.UrlEncode(_url);
            var stringToSign = encodedUrl + "\n" + expiry;
            var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_sharedAccessKey));

            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
            var sasToken = $"SharedAccessSignature sr={encodedUrl}&sig={WebUtility.UrlEncode(signature)}&se={expiry}&skn={_sharedAccessKeyName}";

            request.Headers[HttpRequestHeader.Authorization] = sasToken;

            foreach (var header in headers)
                request.Headers[header.Key] = header.Value;

            using (var stream = await request.GetRequestStreamAsync())
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.Write(payload);
                streamWriter.Flush();
            }

            _log.Info("Sending Apple push notification:\r\n" +
                      $"Url: {request.RequestUri}\r\n" +
                      $"Method: {request.Method}\r\n" +
                      $"ContentType: {request.ContentType}\r\n" +
                      "Headers\r\n" +
                      $"Authorization: {sasToken}\r\n" +
                      string.Concat(headers.Select(p=> $"{p.Key} header: {p.Value}\r\n")) +
                      $"Content\r\n{payload}");
                      
            await request.GetResponseAsync();
        }
    }
}
