using Lykke.Service.PayPushNotifications.Core.Domain;
using Lykke.Service.PayPushNotifications.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.PayPushNotifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IPayLoadBuilderFactory _builderFactory;
        private readonly INotificationClientFactory _clientFactory;
        private readonly IEmployeeNotificationIdRepository _employeeNotificationIdRepository;
        private readonly IMerchantNotificationIdRepository _merchantNotificationIdRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly NotificationPlatform[] _platforms;

        public NotificationService(IPayLoadBuilderFactory builderFactory, 
            INotificationClientFactory clientFactory, 
            IEmployeeNotificationIdRepository employeeNotificationIdRepository,
            IMerchantNotificationIdRepository merchantNotificationIdRepository,
            INotificationRepository notificationRepository)
        {
            _builderFactory = builderFactory;
            _clientFactory = clientFactory;
            _employeeNotificationIdRepository = employeeNotificationIdRepository;
            _merchantNotificationIdRepository = merchantNotificationIdRepository;
            _notificationRepository = notificationRepository;
            _platforms = Enum.GetValues(typeof(NotificationPlatform)).Cast<NotificationPlatform>().ToArray();
        }

        #region Send
        public async Task SendAsync(NotificationMessage notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var notificationIds = await GetNotificationIdsAsync(notification);
            if (!notificationIds.Any())
            {
                var builder = _builderFactory.CreateBuilder(NotificationPlatform.Aps);
                builder.AddMessage(notification.Message);

                var client = _clientFactory.GetClient(NotificationPlatform.Aps);
                await client.SendNotificationAsync(builder.ToString(), new string[0]);
                return;
            }

            var tasks = new List<Task>();
            foreach (var platform in notificationIds.Keys)
            {
                var builder = _builderFactory.CreateBuilder(platform);
                builder.AddMessage(notification.Message);

                var client = _clientFactory.GetClient(platform);
                tasks.Add(client.SendNotificationAsync(builder.ToString(), notificationIds[platform]));
            }

            tasks.Add(_notificationRepository.InsertOrReplaceAsync(new Notification()
            {
                NotificationMessage = notification
            }));
            await Task.WhenAll(tasks);
        }

        private async Task<IDictionary<NotificationPlatform, List<string>>> GetNotificationIdsAsync(
            NotificationMessage notification)
        {
            var notificationIds = new Dictionary<NotificationPlatform, List<string>>();

            await GetEmployeeNotificationIdsAsync(notificationIds, notification.EmployeeIds);
            await GetMerchantNotificationIdsAsync(notificationIds, notification.MerchantIds);

            return notificationIds;
        }

        private async Task GetEmployeeNotificationIdsAsync(
            IDictionary<NotificationPlatform, List<string>> notificationIds,
            string[] employeeIds)
        {
            if (employeeIds != null)
            {
                var employeeNotificationIds = await _employeeNotificationIdRepository.GetAsync(employeeIds);
                foreach (var notificationId in employeeNotificationIds)
                {
                    AddNotificationId(notificationIds, notificationId.Platform, notificationId.NotificationId);
                }
            }
        }

        private async Task GetMerchantNotificationIdsAsync(
            IDictionary<NotificationPlatform, List<string>> notificationIds,
            string[] merchantIds)
        {
            if (merchantIds != null)
            {
                var merchantNotificationIds = await _merchantNotificationIdRepository.GetAsync(merchantIds);
                foreach (var notificationId in merchantNotificationIds)
                {
                    AddNotificationId(notificationIds, notificationId.Platform, notificationId.NotificationId);
                }
            }
        }

        private void AddNotificationId(IDictionary<NotificationPlatform, List<string>> notificationIds,
            NotificationPlatform platform, string notificationId)
        {
            if (!notificationIds.TryGetValue(platform, out var notifications))
            {
                notifications = new List<string>();
                notificationIds[platform] = notifications;
            }

            notifications.Add(notificationId);
        }
        #endregion Send

        #region GetNotificationIds
        public async Task<Dictionary<NotificationPlatform, string[]>> GetNotificationIdsAsync(string employeeId, string merchantId)
        {
            var employeeNotificationIds = await GetEmployeeNotificationIdsAsync(employeeId);
            var merchantNotificationIds = await GetMerchantNotificationIdsAsync(merchantId);

            var result = new Dictionary<NotificationPlatform, string[]>();
            foreach (var platform in _platforms)
            {
                result[platform] = new[]
                {
                    employeeNotificationIds.First(n=>n.Platform== platform).NotificationId,
                    merchantNotificationIds.First(n=>n.Platform== platform).NotificationId
                };
            }

            return result;
        }

        private async Task<IEmployeeNotificationId[]> GetEmployeeNotificationIdsAsync(
            string employeeId)
        {
            if (string.IsNullOrEmpty(employeeId))
            {
                return new IEmployeeNotificationId[0];
            }

            var notificationIds = (await _employeeNotificationIdRepository.GetAsync(employeeId)).ToArray();
            if (notificationIds.Count() >= _platforms.Length)
            {
                return notificationIds;
            }

            var newNotificationIds =
                await AddMissedEmployeeNotificationIdsAsync(employeeId, notificationIds);

            return notificationIds.Union(newNotificationIds).ToArray();
        }

        private async Task<IEmployeeNotificationId[]> AddMissedEmployeeNotificationIdsAsync(
            string employeeId, IEmployeeNotificationId[] notificationIds)
        {
            var newNotificationIds = new List<IEmployeeNotificationId>();

            foreach (var platform in _platforms)
            {
                if (notificationIds.Any(i => i.Platform == platform))
                {
                    continue;
                }

                newNotificationIds.Add(new EmployeeNotificationId
                {
                    EmployeeId = employeeId,
                    NotificationId = Guid.NewGuid().ToString(),
                    Platform = platform
                });
            }

            await _employeeNotificationIdRepository.InsertOrReplaceBatchAsync(newNotificationIds);
            return newNotificationIds.ToArray();
        }

        private async Task<IMerchantNotificationId[]> GetMerchantNotificationIdsAsync(
            string merchantId)
        {
            if (string.IsNullOrEmpty(merchantId))
            {
                return new IMerchantNotificationId[0];
            }

            var notificationIds = (await _merchantNotificationIdRepository.GetAsync(merchantId)).ToArray();
            if (notificationIds.Count() >= _platforms.Length)
            {
                return notificationIds;
            }

            var newNotificationIds =
                await AddMissedMerchantNotificationIdsAsync(merchantId, notificationIds);

            return notificationIds.Union(newNotificationIds).ToArray();
        }

        private async Task<List<IMerchantNotificationId>> AddMissedMerchantNotificationIdsAsync(
            string merchantId, IMerchantNotificationId[] notificationIds)
        {
            var newNotificationIds = new List<IMerchantNotificationId>();

            foreach (var platform in _platforms)
            {
                if (notificationIds.Any(i => i.Platform == platform))
                {
                    continue;
                }

                newNotificationIds.Add(new MerchantNotificationId
                {
                    MerchantId = merchantId,
                    NotificationId = Guid.NewGuid().ToString(),
                    Platform = platform
                });
            }

            await _merchantNotificationIdRepository.InsertOrReplaceBatchAsync(newNotificationIds);
            return newNotificationIds;
        }

        #endregion GetNotificationIds
    }
}
