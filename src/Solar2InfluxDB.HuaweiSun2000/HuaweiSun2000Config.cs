namespace Solar2InfluxDB.HuaweiSun2000
{
    public class Config
    {
        public const string ConfigSection = "HuaweiSun2000";

        public string Hostname { get; set; }

        public ParameterConfig Inverter { get; set; }

        public ParameterConfig PowerMeter { get; set; }

        public ParameterConfig PVStrings { get; set; }
    }

    public class ParameterConfig
    {
        public string[] ParametersToRead { get; set; }

    }
}
