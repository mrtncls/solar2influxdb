using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class MeasurementMapper
    {
        public static DoubleMeasurement GetMeasurement(this HuaweiSun2000Client client, string name, double value) 
            => new DoubleMeasurement(name, value);

        public static IntegerMeasurement GetMeasurement(this HuaweiSun2000Client client, string name, int value) 
            => new IntegerMeasurement(name, value);
    }
}
