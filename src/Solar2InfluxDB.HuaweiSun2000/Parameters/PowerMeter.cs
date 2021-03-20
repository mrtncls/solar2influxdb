using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000
{
    internal static class PowerMeter
    {
        public static Dictionary<string, Func<HuaweiSun2000Client, string, Measurement>> Parameters = new Dictionary<string, Func<HuaweiSun2000Client, string, Measurement>>
        {
            ["Voltage A [V]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37101) / 10),
            ["Voltage B [V]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37103) / 10),
            ["Voltage C [V]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37105) / 10),
            ["Current A [A]"] = (c, n) => new DoubleMeasurement(n, -(double)c.GetInteger(37107) / 100),
            ["Current B [A]"] = (c, n) => new DoubleMeasurement(n, -(double)c.GetInteger(37109) / 100),
            ["Current C [A]"] = (c, n) => new DoubleMeasurement(n, -(double)c.GetInteger(37111) / 100),
            ["Power [W]"] = (c, n) => new IntegerMeasurement(n, -c.GetInteger(37113)),
            ["Reactive power [VAR]"] = (c, n) => new IntegerMeasurement(n, -c.GetInteger(37115)),
            ["Power factor"] = (c, n) => new DoubleMeasurement(n, -(double)c.GetShort(37117) / 1000),
            ["Frequency [Hz]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetShort(37118) / 100),
            ["Exported energy [kWh]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37119) / 100),
            ["Accumulated energy [kWh]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetUnsignedInteger(37121) / 100),
            ["Voltage AB [V]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37126) / 10),
            ["Voltage BC [V]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37128) / 10),
            ["Voltage CA [V]"] = (c, n) => new DoubleMeasurement(n, (double)c.GetInteger(37130) / 10),
            ["Power A [W]"] = (c, n) => new IntegerMeasurement(n, -c.GetInteger(37132)),
            ["Power B [W]"] = (c, n) => new IntegerMeasurement(n, -c.GetInteger(37134)),
            ["Power C [W]"] = (c, n) => new IntegerMeasurement(n, -c.GetInteger(37136)),
        };

        public static Task<MeasurementCollection> GetPowerMeterMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .Select(p => Parameters[p](client, p))
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "Power meter",
                    measurements,
                    ("Hostname", client.Hostname)));
        }
    }
}
