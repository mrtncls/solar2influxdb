using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHuaweiSun2000(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<Config>(configuration.GetSection(Config.ConfigSection))
                .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<Config>>().Value)
                .AddSingleton<IMeasurementReader, HuaweiSun2000Client>();
    }
}
