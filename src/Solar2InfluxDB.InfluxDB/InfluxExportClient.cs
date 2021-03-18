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

        public InfluxExportClient(InfluxDBConfig config, ILogger<InfluxExportClient> logger)
        {
            this.logger = logger;

            client = InfluxDBClientFactory.CreateV1(
                config.Url ?? throw new Exception("No InfluxDB url specified"),
                config.Username ?? string.Empty,
                config.Password?.ToCharArray() ?? Array.Empty<char>(),
                config.Database ?? throw new Exception("No InfluxDB database specified"),
                config.RetentionPolicy ?? "autogen");
        }

        Task IMeasurementWriter.Initialize() => Task.CompletedTask;

        Task IMeasurementWriter.Write(MeasurementCollection measurements)
        {
            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoint(measurements.ToPointData());

                logger.LogDebug($"Collection '{measurements.Name}' written to InfluxDB");
            }

            return Task.CompletedTask;
        }
    }
}
