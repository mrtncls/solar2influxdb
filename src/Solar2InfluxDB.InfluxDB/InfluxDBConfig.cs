namespace Solar2InfluxDB.InfluxDB
{
    public class InfluxDBConfig
    {
        public const string ConfigSection = "InfluxDB";

        public string Url { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Database { get; set; }

        public string RetentionPolicy { get; set; }
    }
}
