using Autofac;
using AutoMapper;
using AzureStorage.Tables;
using Common;
using Lykke.Common.Log;
using Lykke.Logs;
using Lykke.Service.PayPushNotifications.AzureRepositories.EmployeeNotificationIds;
using Lykke.Service.PayPushNotifications.AzureRepositories.MerchantNotificationIds;
using Lykke.Service.PayPushNotifications.AzureRepositories.Notifications;
using Lykke.Service.PayPushNotifications.Core.Domain;
using Lykke.Service.PayPushNotifications.Core.Services;
using Lykke.Service.PayPushNotifications.Rabbit;
using Lykke.Service.PayPushNotifications.Services;
using Lykke.Service.PayPushNotifications.Settings;
using Lykke.SettingsReader;
using System.Collections.Generic;
using NotificationPlatform = Lykke.Service.PayPushNotifications.Core.Domain.NotificationPlatform;

namespace Lykke.Service.PayPushNotifications.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;
        private readonly ILogFactory _logFactory;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _logFactory = LogFactory.Create();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Do not register entire settings in container, pass necessary settings to services which requires them
            //builder.RegisterInstance(_log)
            //    .As<ILog>()
            //    .SingleInstance();

            var mapperProvider = new MapperProvider();
            IMapper mapper = mapperProvider.GetMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            builder.RegisterInstance<IEmployeeNotificationIdRepository>(
                new EmployeeNotificationIdRepository(
                    AzureTableStorage<EmployeeNotificationIdEntity>.Create(
                        _appSettings.ConnectionString(x => x.PayPushNotificationsService.Db.DataConnString),
                        _appSettings.CurrentValue.PayPushNotificationsService.Db.EmployeeNotificationIdsTableName,
                        _logFactory)));


            builder.RegisterInstance<IMerchantNotificationIdRepository>(
                new MerchantNotificationIdRepository(
                    AzureTableStorage<MerchantNotificationIdEntity>.Create(
                        _appSettings.ConnectionString(x => x.PayPushNotificationsService.Db.DataConnString),
                        _appSettings.CurrentValue.PayPushNotificationsService.Db.MerchantNotificationIdsTableName,
                        _logFactory)));

            builder.RegisterInstance<INotificationRepository>(
                new NotificationRepository(
                    AzureTableStorage<NotificationEntity>.Create(
                        _appSettings.ConnectionString(x => x.PayPushNotificationsService.Db.DataConnString),
                        _appSettings.CurrentValue.PayPushNotificationsService.Db.NotificationsTableName, _logFactory)));

            builder.RegisterType<PayLoadBuilderFactory>()
                .As<IPayLoadBuilderFactory>()
                .SingleInstance();

            builder.RegisterType<NotificationClientFactory>()
                .As<INotificationClientFactory>()
                .SingleInstance()
                .WithParameter("clients",
                    new Dictionary<NotificationPlatform, INotificationClient>
                    {
                        {
                            NotificationPlatform.Aps,
                            AppleNotificationClient.CreateClientFromConnectionString(
                                _appSettings.CurrentValue.PayPushNotificationsService.HubConnectionString,
                                _appSettings.CurrentValue.PayPushNotificationsService.HubName)
                        }
                    });

            builder.RegisterType<NotificationService>()
                .As<INotificationService>()
                .SingleInstance();

            builder.RegisterType<NotificationSubscriber>()
                .As<IStartable>()
                .As<IStopable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter("settings", _appSettings.CurrentValue.PayPushNotificationsService.Rabbit);
        }
    }
}
