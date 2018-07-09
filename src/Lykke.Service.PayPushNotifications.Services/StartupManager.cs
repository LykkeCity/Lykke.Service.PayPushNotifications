using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Sdk;
using System;
using System.Threading.Tasks;
using Autofac;

namespace Lykke.Service.PayPushNotifications.Services
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly ILog _log;
        private readonly NotificationSubscriber _notificationSubscriber;

        public StartupManager([NotNull] ILogFactory logFactory,
            NotificationSubscriber notificationSubscriber)
        {
            _log = logFactory?.CreateLog(this) ?? throw new ArgumentNullException(nameof(logFactory));
            _notificationSubscriber =
                notificationSubscriber ?? throw new ArgumentNullException(nameof(notificationSubscriber));
        }

        public Task StartAsync()
        {
            StartComponentAsync(_notificationSubscriber);

            return Task.CompletedTask;
        }

        private void StartComponentAsync(object component, string componentDisplayName = null)
        {
            if (string.IsNullOrEmpty(componentDisplayName))
            {
                componentDisplayName = component.GetType().Name;
            }
            
            _log.Info($"Starting {componentDisplayName} ...");

            if (component is IStartable startableComponent)
            {
                startableComponent.Start();

                _log.Info($"{componentDisplayName} successfully started.");
            }
            else
            {
                _log.Warning("Component can not be started.");
            }
        }
    }
}
