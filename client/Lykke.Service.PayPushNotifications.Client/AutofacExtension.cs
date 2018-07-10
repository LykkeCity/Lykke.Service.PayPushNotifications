using System;
using Autofac;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.HttpClientGenerator;
using Lykke.Service.PayPushNotifications.Client.Publisher;

namespace Lykke.Service.PayPushNotifications.Client
{
    [PublicAPI]
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers <see cref="IPayPushNotificationsClient"/> in Autofac container using <see cref="PayPushNotificationsServiceClientSettings"/>.
        /// </summary>
        /// <param name="builder">Autofac container builder.</param>
        /// <param name="settings">PayPushNotifications client settings.</param>
        /// <param name="builderConfigure">Optional <see cref="HttpClientGeneratorBuilder"/> configure handler.</param>
        public static void RegisterPayPushNotificationsClient(
            [NotNull] this ContainerBuilder builder,
            [NotNull] PayPushNotificationsServiceClientSettings settings,
            [CanBeNull] Func<HttpClientGeneratorBuilder, HttpClientGeneratorBuilder> builderConfigure)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(PayPushNotificationsServiceClientSettings.ServiceUrl));

            builder.RegisterClient<IPayPushNotificationsClient>(settings?.ServiceUrl, builderConfigure);
        }

        public static void RegisterPayPushNotificationPublisher(this ContainerBuilder builder,
            RabbitMqPublisherSettings settings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            builder.RegisterType<NotificationPublisher>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .UsingConstructor(typeof(RabbitMqPublisherSettings), typeof(ILogFactory))
                .WithParameter("settings", settings);
        }

        [Obsolete]
        public static void RegisterPayPushNotificationPublisher(this ContainerBuilder builder,
            RabbitMqPublisherSettings settings, ILog log)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            builder.RegisterType<NotificationPublisher>()
                .AsSelf()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .UsingConstructor(typeof(RabbitMqPublisherSettings),typeof(ILog))
                .WithParameter("settings", settings)
                .WithParameter("log", log); ;
        }
    }
}
