using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Logs;
using Lykke.Service.PayPushNotifications.Client.Publisher;
using Xunit;

namespace Lykke.Service.PayPushNotifications.Tests
{
    public class Class1
    {
        //    [Fact]
        //    public async Task Test1()
        //    {
        //        try
        //        {
        //            var logFactory = LogFactory.Create();
        //            using (var publisher = new NotificationPublisher(new RabbitMqPublisherSettings
        //            {
        //                ConnectionString = "amqp://lykke.history:lykke.history@rabbit-main.rabbits.svc.cluster.local:5672",
        //                ExchangeName = "pay.push"
        //            }, logFactory))
        //            {
        //                publisher.Start();

        //                await publisher.PublishAsync(new NotificationMessage
        //                {
        //                    MerchantIds = new[] { "pem3" },
        //                    Message = "test message"
        //                });

        //                publisher.Stop();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            var a = 1;
        //        }
        //    }
    }
}
