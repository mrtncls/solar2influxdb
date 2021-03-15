using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.Model;
using System;

namespace Solar2InfluxDB.InfluxDB
{
    public class InfluxExportClient
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

        public void Write<TValue>(Measurement<TValue> measurement)
        {
            using (var writeApi = client.GetWriteApi())
            {
                writeApi.WritePoint(measurement.ToPointData());

                logger.LogTrace($"Point {measurement.Name} written to InfluxDB");
            }
        }
    }
}
