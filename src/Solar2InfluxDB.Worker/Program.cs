using System;
using System.IO;
using LoggingAdvanced.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.HuaweiSun2000;
using Solar2InfluxDB.InfluxDB;

namespace Solar2InfluxDB.Worker
{
    class Program
    {
        private const string SettingsFile = "appsettings.json";
        private const string ConfigPathInContainer = "/config/";

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
                    if (RunningInContainer)
                    {
                        if (Directory.Exists(ConfigPathInContainer))
                        {
                            string SettingsFileInContainer = Path.Combine(ConfigPathInContainer, SettingsFile);
                            if (!File.Exists(SettingsFileInContainer))
                            {
                                File.Copy(SettingsFile, SettingsFileInContainer);
                                Console.WriteLine($"Copied default settings file to {ConfigPathInContainer}");
                            }

                            builder.SetBasePath(ConfigPathInContainer);
                        }
                        else
                        {
                            Console.WriteLine("Consider mounting a volume on /config to use your own settings. Using default settings... ");
                        }
                    }

                    builder.AddJsonFile(SettingsFile);

                    Configuration = builder.Build();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsoleAdvanced(Configuration.GetSection("Logging"));
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .AddHuaweiSun2000(Configuration)
                        .AddInfluxDB(Configuration)
                        .AddHostedService<Worker>();
                });
        }

        private static bool RunningInContainer => Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
    }
}
