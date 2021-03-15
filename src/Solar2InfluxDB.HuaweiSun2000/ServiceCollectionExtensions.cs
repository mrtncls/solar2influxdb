using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHuaweiSun2000(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<HuaweiSun2000Config>(configuration.GetSection(HuaweiSun2000Config.ConfigSection))
                .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<HuaweiSun2000Config>>().Value)
                .AddSingleton<HuaweiSun2000Client>();
    }
}
