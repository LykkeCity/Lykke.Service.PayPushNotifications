using System;
using Autofac;
using Common;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker.Publisher;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.PayPushNotifications.Client.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Common.Log;

namespace Lykke.Service.PayPushNotifications.Client.Publisher
{
    public class NotificationPublisher: IStartable, IStopable
    {
        private readonly RabbitMqPublisherSettings _settings;
        private readonly ILogFactory _logFactory;
        private readonly ILog _log;
        private RabbitMqPublisher<INotificationMessage> _publisher;

        [Obsolete]
        public NotificationPublisher(RabbitMqPublisherSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        public NotificationPublisher(RabbitMqPublisherSettings settings, ILogFactory logFactory)
        {
            _settings = settings;
            _logFactory = logFactory;
            _log = _logFactory.CreateLog(this);
        }

        public void Start()
        {
            var settings =
                RabbitMqSubscriptionSettings.CreateForPublisher(_settings.ConnectionString,
                    _settings.ExchangeName);
            settings.MakeDurable();

            if (_logFactory == null)
            {
                _publisher = new RabbitMqPublisher<INotificationMessage>(settings)
                    .SetConsole(new LogToConsole())
                    .SetLogger(_log);
            }
            else
            {
                _publisher = new RabbitMqPublisher<INotificationMessage>(_logFactory, settings);
            }

            _publisher.DisableInMemoryQueuePersistence()
                .PublishSynchronously()
                .SetSerializer(new JsonMessageSerializer<INotificationMessage>())
                .SetPublishStrategy(new DefaultFanoutPublishStrategy(settings))
                .Start();
        }

        public async Task PublishAsync(INotificationMessage notificationMessage)
        {
            Validate(notificationMessage);

            await Task.WhenAll(_publisher.ProduceAsync(notificationMessage));
        }

        protected virtual void Validate(INotificationMessage notificationMessage)
        {
            var context = new ValidationContext(notificationMessage);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(notificationMessage, context, results, true);

            if (!isValid)
            {
                var modelErrors = new Dictionary<string, List<string>>();
                foreach (ValidationResult validationResult in results)
                {
                    if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                    {
                        foreach (string memberName in validationResult.MemberNames)
                        {
                            AddModelError(modelErrors, memberName, validationResult.ErrorMessage);
                        }
                    }
                    else
                    {
                        AddModelError(modelErrors, string.Empty, validationResult.ErrorMessage);
                    }
                }

                throw new PayPushNotificationsApiException("Model is invalid.", modelErrors);
            }
        }

        private void AddModelError(Dictionary<string, List<string>> modelErrors, string memberName,
            string errorMessage)
        {
            if (!modelErrors.TryGetValue(memberName, out var errors))
            {
                errors = new List<string>();
                modelErrors[memberName] = errors;
            }

            errors.Add(errorMessage);
        }

        public void Dispose()
        {
            _publisher?.Dispose();
        }

        public void Stop()
        {
            _publisher?.Stop();
        }
    }
}
