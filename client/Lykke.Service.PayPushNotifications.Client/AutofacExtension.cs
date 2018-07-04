using System;
using Autofac;
using JetBrains.Annotations;
using Lykke.HttpClientGenerator;

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
    }
}
