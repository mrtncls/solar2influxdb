using LoggingAdvanced.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Solar2InfluxDB.HuaweiSun2000;
using Solar2InfluxDB.InfluxDB;

namespace Solar2InfluxDB.Worker
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfiguration Configuration = null;

            return Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(builder =>
                {
                    builder.AddJsonFile("appsettings.json");

                    Configuration = builder.Build();
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsoleAdvanced(context.Configuration.GetSection("Logging"));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddHuaweiSun2000(Configuration)
                        .AddInfluxDB(Configuration)
                        .AddHostedService<Worker>();
                });
        }
    }
}
