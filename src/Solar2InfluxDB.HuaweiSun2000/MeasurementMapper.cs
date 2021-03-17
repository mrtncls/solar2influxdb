using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class MeasurementMapper
    {
        public static DoubleMeasurement GetMeasurement(this HuaweiSun2000Client client, string name, double value) 
            => new DoubleMeasurement(name, value, GetTags(client));

        public static IntegerMeasurement GetMeasurement(this HuaweiSun2000Client client, string name, int value) 
            => new IntegerMeasurement(name, value, GetTags(client));

        private static (string name, string value)[] GetTags(this HuaweiSun2000Client client)
            => new[]
            {
                ("hostname", client.Hostname),
                ("model", client.Model),
                ("serialnumber", client.SerialNumber)
            };
    }
}
