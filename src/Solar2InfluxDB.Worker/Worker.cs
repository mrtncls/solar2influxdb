using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.HuaweiSun2000;
using Solar2InfluxDB.InfluxDB;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solar2InfluxDB.Worker
{
    public class Worker : IHostedService
    {
        private readonly HuaweiSun2000Client solarClient;
        private readonly InfluxExportClient influxClient;
        private readonly ILogger<Worker> logger;
        private readonly CancellationTokenSource timerSource;

        private Task workerTask;

        public Worker(
            HuaweiSun2000Client solarClient,
            InfluxExportClient influxClient,
            ILogger<Worker> logger)
        {
            this.solarClient = solarClient;
            this.influxClient = influxClient;
            this.logger = logger;
            timerSource = new CancellationTokenSource();
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            await solarClient.Initialize();

            workerTask = Task.Run(() => ForwardMeasurements());

            logger.LogInformation("Worker started");
        }

        async Task ForwardMeasurements()
        {
            while (!timerSource.Token.IsCancellationRequested)
            {
                try
                {
                    influxClient.Write(solarClient.GetRatedPower());
                    influxClient.Write(solarClient.GetInputPower());
                    influxClient.Write(solarClient.GetActivePower());
                    influxClient.Write(solarClient.GetReactivePower());
                    influxClient.Write(solarClient.GetPowerMeterActivePower());

                    logger.LogDebug("Measurements forwarded");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Forwarding measurement failed: {e.GetBaseException().Message}");

                    await solarClient.Initialize();
                }

                await Task.Delay(TimeSpan.FromSeconds(5), timerSource.Token);
            }
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            timerSource.Cancel();

            await workerTask;

            logger.LogInformation("Worker stoppped");
        }
    }
}
