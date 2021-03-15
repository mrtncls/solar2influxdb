using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class MeasurementMapper
    {
        public static Measurement<TValue> GetMeasurement<TValue>(this HuaweiSun2000Client client, string name, TValue value)
        {
            var measurement = new Measurement<TValue>
            {
                Name = name,
                Value = value,
            };

            measurement.Tags["hostname"] = client.Hostname;

            return measurement;
        }
    }
}
