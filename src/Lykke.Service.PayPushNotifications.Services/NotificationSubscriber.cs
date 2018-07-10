using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PayPushNotifications.Core.Domain;
using Lykke.Service.PayPushNotifications.Core.Services;
using Lykke.Service.PayPushNotifications.Core.Settings;

namespace Lykke.Service.PayPushNotifications.Services
{
    public class NotificationSubscriber : IStartable, IStopable
    {
        private RabbitMqSubscriber<NotificationMessage> _subscriber;
        private readonly IRabbitMqSubscriberSettings _settings;
        private readonly ILogFactory _logFactory;
        private readonly INotificationService _notificationService;

        public NotificationSubscriber(INotificationService notificationService,
            IRabbitMqSubscriberSettings settings, ILogFactory logFactory)
        {
            _notificationService = notificationService;
            _settings = settings;
            _logFactory = logFactory;
        }

        public void Start()
        {
            var settings = new RabbitMqSubscriptionSettings
            {
                ConnectionString = _settings.ConnectionString,
                ExchangeName = _settings.ExchangeName,
                QueueName = _settings.QueueName,
                IsDurable = true
            };

            var errorHandlingStrategy = new ResilientErrorHandlingStrategy(_logFactory, settings,
                TimeSpan.FromSeconds(10),
                next: new DeadQueueErrorHandlingStrategy(_logFactory, settings));

            _subscriber = new RabbitMqSubscriber<NotificationMessage>(_logFactory, settings,
                    errorHandlingStrategy)
                .SetMessageDeserializer(new JsonMessageDeserializer<NotificationMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private Task ProcessMessageAsync(NotificationMessage notification)
        {
            return _notificationService.SendAsync(notification);
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }
    }
}
