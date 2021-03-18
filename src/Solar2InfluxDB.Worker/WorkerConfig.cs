namespace Solar2InfluxDB.Worker
{
    public class WorkerConfig
    {
        public const string ConfigSection = "Worker";

        public int IntervalInSeconds { get; set; } = 5;
    }
}
