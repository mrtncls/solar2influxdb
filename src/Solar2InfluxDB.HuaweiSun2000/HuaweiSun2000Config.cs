namespace Solar2InfluxDB.HuaweiSun2000
{
    public class HuaweiSun2000Config
    {
        public const string ConfigSection = "HuaweiSun2000";

        public string Hostname { get; set; }

        public PowerMeterConfig PowerMeter { get; set; }
    }

    public class PowerMeterConfig
    {
        public string[] ParametersToRead { get; set; }
    }
}
