using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Solar2InfluxDB.Worker
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<WorkerConfig>(configuration.GetSection(WorkerConfig.ConfigSection))
                .AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<WorkerConfig>>().Value)
                .AddSingleton<MeasurementChangedTracker>()
                .AddHostedService<Worker>();
    }
}
