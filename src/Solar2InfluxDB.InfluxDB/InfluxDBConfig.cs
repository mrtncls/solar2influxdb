namespace Solar2InfluxDB.InfluxDB
{
    public class InfluxDBConfig
    {
        public const string ConfigSection = "InfluxDB";

        public string Url { get; set; }

        public string Token { get; set; }

        public string Organization { get; set; }

        public string Bucket { get; set; }

        public bool V1 { get; set; }

        public string V1_Username { get; set; }

        public string V1_Password { get; set; }

        public string V1_Database { get; set; }

        public string V1_RetentionPolicy { get; set; }
    }
}
