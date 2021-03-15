using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Solar2InfluxDB.InfluxDB
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfluxDB(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<InfluxDBConfig>(configuration.GetSection(InfluxDBConfig.ConfigSection))
                .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<InfluxDBConfig>>().Value)
                .AddSingleton<InfluxExportClient>();
    }
}
