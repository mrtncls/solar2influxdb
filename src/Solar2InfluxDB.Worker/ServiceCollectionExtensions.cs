using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Solar2InfluxDB.Worker
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorker(this IServiceCollection services, IConfiguration configuration)
            => services
                .Configure<WorkerConfig>(configuration.GetSection(WorkerConfig.ConfigSection))
                .AddSingleton<MeasurmentChangedTracker>()
                .AddHostedService<Worker>();
    }
}
