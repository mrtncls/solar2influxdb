using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.HuaweiSun2000;
using Solar2InfluxDB.InfluxDB;
using Solar2InfluxDB.Model;
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
        private readonly MeasurementChangedTracker measurmentChangedTracker;

        private Task workerTask;

        public Worker(
            HuaweiSun2000Client solarClient,
            InfluxExportClient influxClient,
            MeasurementChangedTracker measurmentChangedTracker,
            ILogger<Worker> logger)
        {
            this.solarClient = solarClient;
            this.influxClient = influxClient;
            this.logger = logger;
            timerSource = new CancellationTokenSource();
            this.measurmentChangedTracker = measurmentChangedTracker;
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            await solarClient.Initialize();

            workerTask = Task.Run(() => ProcessMeasurements());

            logger.LogInformation("Worker started");
        }

        async Task ProcessMeasurements()
        {
            while (!timerSource.Token.IsCancellationRequested)
            {
                try
                {
                    Process(solarClient.GetRatedPower());
                    Process(solarClient.GetInputPower());
                    Process(solarClient.GetActivePower());
                    Process(solarClient.GetReactivePower());
                    Process(solarClient.GetPowerMeterActivePower());

                    logger.LogDebug("Measurements processed");
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Process measurement failed: {e.GetBaseException().Message}");

                    await solarClient.Initialize();
                }

                await Task.Delay(TimeSpan.FromSeconds(5), timerSource.Token);
            }
        }

        private void Process(Measurement measurement)
        {
            if (measurmentChangedTracker.IsMeasurementChanged(measurement))
            {
                influxClient.Write(measurement);
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
