using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solar2InfluxDB.Worker
{
    public class Worker : IHostedService
    {
        private readonly IMeasurementReader measurementReader;
        private readonly IMeasurementWriter measurementWriter;
        private readonly ILogger<Worker> logger;
        private readonly CancellationTokenSource timerSource;
        private readonly TimeSpan Interval;
        private readonly TimeSpan InitDelayAfterException = TimeSpan.FromSeconds(5);

        private Task workerTask;

        public Worker(
            IMeasurementReader measurementReader,
            IMeasurementWriter measurementWriter,
            WorkerConfig config,
            ILogger<Worker> logger)
        {
            this.measurementReader = measurementReader;
            this.measurementWriter = measurementWriter;
            this.logger = logger;
            timerSource = new CancellationTokenSource();
            Interval = TimeSpan.FromSeconds(config.IntervalInSeconds);
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            workerTask = Task.Run(() => ProcessMeasurements());

            logger.LogInformation("Worker started");

            return Task.CompletedTask;
        }

        async Task ProcessMeasurements()
        {
            while (!timerSource.Token.IsCancellationRequested)
            {
                try
                {
                    await measurementReader.Initialize();
                    await measurementWriter.Initialize();

                    while (!timerSource.Token.IsCancellationRequested)
                    {
                        await foreach (var measurements in measurementReader.ReadMeasurementsFromDevices())
                        {
                            await measurementWriter.Write(measurements);
                        }

                        logger.LogDebug("Measurements processed");

                        await Task.Delay(Interval, timerSource.Token);
                    }
                }
                catch (Exception e)
                {
                    logger.LogError(e, $"Process measurement failed: {e.GetBaseException().Message}");

                    await Task.Delay(InitDelayAfterException, timerSource.Token);
                }
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
