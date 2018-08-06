using Autofac;
using AutoMapper;
using AzureStorage.Tables;
using Common;
using Lykke.Common.Log;
using Lykke.Service.PayPushNotifications.AzureRepositories.EmployeeNotificationIds;
using Lykke.Service.PayPushNotifications.AzureRepositories.MerchantNotificationIds;
using Lykke.Service.PayPushNotifications.AzureRepositories.Notifications;
using Lykke.Service.PayPushNotifications.Core.Domain;
using Lykke.Service.PayPushNotifications.Core.Services;
using Lykke.Service.PayPushNotifications.Services;
using Lykke.Service.PayPushNotifications.Settings;
using Lykke.SettingsReader;
using System.Collections.Generic;
using Lykke.Sdk;
using NotificationPlatform = Lykke.Service.PayPushNotifications.Core.Domain.NotificationPlatform;

namespace Lykke.Service.PayPushNotifications.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _appSettings;

        public ServiceModule(IReloadingManager<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Do not register entire settings in container, pass necessary settings to services which requires them
            var mapperProvider = new MapperProvider();
            IMapper mapper = mapperProvider.GetMapper();
            builder.RegisterInstance(mapper).As<IMapper>();

            builder.Register(c =>
                    new EmployeeNotificationIdRepository(
                        AzureTableStorage<EmployeeNotificationIdEntity>.Create(
                            _appSettings.ConnectionString(x => x.PayPushNotificationsService.Db.DataConnString),
                            _appSettings.CurrentValue.PayPushNotificationsService.Db.EmployeeNotificationIdsTableName,
                            c.Resolve<ILogFactory>())))
                .As<IEmployeeNotificationIdRepository>()
                .SingleInstance();

            builder.Register(c=>
                new MerchantNotificationIdRepository(
                    AzureTableStorage<MerchantNotificationIdEntity>.Create(
                        _appSettings.ConnectionString(x => x.PayPushNotificationsService.Db.DataConnString),
                        _appSettings.CurrentValue.PayPushNotificationsService.Db.MerchantNotificationIdsTableName,
                        c.Resolve<ILogFactory>())))
            .As<IMerchantNotificationIdRepository>()    
            .SingleInstance();

            builder.Register(c=>
                new NotificationRepository(
                    AzureTableStorage<NotificationEntity>.Create(
                        _appSettings.ConnectionString(x => x.PayPushNotificationsService.Db.DataConnString),
                        _appSettings.CurrentValue.PayPushNotificationsService.Db.NotificationsTableName,
                        c.Resolve<ILogFactory>())))
            .As<INotificationRepository>()
            .SingleInstance();

            builder.RegisterType<PayLoadBuilderFactory>()
                .As<IPayLoadBuilderFactory>()
                .SingleInstance();

            builder.Register(c => new NotificationClientFactory(
                    new Dictionary<NotificationPlatform, INotificationClient>
                    {
                        {
                            NotificationPlatform.Aps,
                            AppleNotificationClient.CreateClientFromConnectionString(
                                _appSettings.CurrentValue.PayPushNotificationsService.HubConnectionString,
                                _appSettings.CurrentValue.PayPushNotificationsService.HubName,
                                c.Resolve<ILogFactory>())
                        }
                    }))
                .As<INotificationClientFactory>()
                .SingleInstance();

            builder.RegisterType<NotificationService>()
                .As<INotificationService>()
                .SingleInstance();

            builder.RegisterType<NotificationSubscriber>()
                .AsSelf()
                .As<IStopable>()
                .SingleInstance()
                .WithParameter("settings", _appSettings.CurrentValue.PayPushNotificationsService.Rabbit);

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();
        }
    }
}
