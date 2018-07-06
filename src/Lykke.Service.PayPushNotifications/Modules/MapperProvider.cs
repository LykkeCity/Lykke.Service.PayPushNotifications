using AutoMapper;
using AutoMapper.Configuration;

namespace Lykke.Service.PayPushNotifications.Modules
{
    public class MapperProvider
    {
        public IMapper GetMapper()
        {
            var mce = new MapperConfigurationExpression();

            var mc = new MapperConfiguration(mce);
            mc.AssertConfigurationIsValid();

            return new Mapper(mc);
        }
    }
}
