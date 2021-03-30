namespace Solar2InfluxDB.HuaweiSun2000
{
    public class Config
    {
        public const string ConfigSection = "HuaweiSun2000";

        public string Hostname { get; set; }

        public ParameterConfig Battery { get; set; }

        public ParameterConfig Inverter { get; set; }

        public ParameterConfig InverterAlarm { get; set; }

        public ParameterConfig InverterState { get; set; }

        public ParameterConfig PowerMeter { get; set; }

        public ParameterConfig PVStrings { get; set; }

        public CustomParameter[] Custom {get;set;}
    }

    public class ParameterConfig
    {
        public string[] ParametersToRead { get; set; }
    }

    public class CustomParameter
    {
        public string Name { get; set; }
        public int Address { get; set; }
        public ParameterType Type { get; set; }
    }

    public enum ParameterType
    {
        Short,
        UnsignedShort,
        Integer,
        UnsignedInteger
    }
}
