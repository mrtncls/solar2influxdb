using InfluxDB.Client;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.Model;
using System;
using System.Threading.Tasks;

namespace Solar2InfluxDB.InfluxDB
{
    public class InfluxExportClient : IMeasurementWriter
    {
        private readonly ILogger<InfluxExportClient> logger;
        private readonly InfluxDBClient client;
        private readonly InfluxDBConfig config;

        public InfluxExportClient(InfluxDBConfig config, ILogger<InfluxExportClient> logger)
        {
            this.logger = logger;
            this.config = config;

            if (config.V1)
            {
                client = InfluxDBClientFactory.CreateV1(
                    config.Url ?? throw new Exception("No InfluxDB url specified"),
                    config.V1_Username ?? string.Empty,
                    config.V1_Password?.ToCharArray() ?? Array.Empty<char>(),
                    config.V1_Database ?? throw new Exception("No InfluxDB database specified"),
                    config.V1_RetentionPolicy ?? "autogen");
            }
            else
            {
                client = InfluxDBClientFactory.Create(config.Url, config.Token);
            }
        }

        Task IMeasurementWriter.Initialize() => Task.CompletedTask;

        Task IMeasurementWriter.Write(MeasurementCollection measurements)
        {
            using (var writeApi = client.GetWriteApi())
            {
                if (config.V1)
                {
                    writeApi.WritePoint(measurements.ToPointData());
                }
                else
                {
                    writeApi.WritePoint(
                        config.Bucket,
                        config.Organization,
                        measurements.ToPointData());
                }

                logger.LogDebug($"Collection '{measurements.Name}' written to InfluxDB");
            }

            return Task.CompletedTask;
        }
    }
}
