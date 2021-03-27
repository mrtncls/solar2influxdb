using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000.Parameters
{
    internal static class Battery
    {
        private static Dictionary<string, Func<HuaweiSun2000Client, string, IEnumerable<Measurement>>> Parameters = new Dictionary<string, Func<HuaweiSun2000Client, string, IEnumerable<Measurement>>>
        {
            ["Status"] = (c, n) => new[] { new StringMeasurement(n, ((BatteryRunningStatus)c.GetUnsignedShort(37000)).ToString()) },
            ["Mode"] = (c, n) => new[] { new StringMeasurement(n, ((BatteryWorkingMode)c.GetUnsignedShort(47004)).ToString()) },
            ["Charge and discharge power [W]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37001)) },
            ["Charge power today [kWh]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(37015) / 100) },
            ["Discharge power today [kWh]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(37017) / 100) },
            ["LCOE"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(47069) / 1000) },
            ["Maximum charge power [W]"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(47075)) },
            ["Maximum discharge power [W]"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(47077)) },
            ["Power limit of grid-tied point [W]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(47079)) },
            ["Charge cutoff capacity [%]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(47081) / 10) },
            ["Discharge cutoff capacity [%]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(47082) / 10) },
            ["Forced charging and discharging period [min]"] = (c, n) => new[] { new UnsignedShortMeasurement(n, c.GetUnsignedShort(47083)) },
            ["Forced charging and discharging power [W]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(47084)) },
        };

        public static Task<MeasurementCollection> GetBatteryMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .SelectMany(p => Parameters[p](client, p))
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "Battery",
                    measurements,
                    ("Hostname", client.Hostname)));
        }
    }

    internal enum BatteryRunningStatus
    {
        Offline = 0,
        Standby = 1,
        Running = 2,
        Fault = 3,
        SleepMode = 4,
    }

    internal enum BatteryWorkingMode
    {
        Unlimited = 0,
        GridConnectionWithZeroPower = 1,
        GridConnectionWithLimitedPower = 2,
    }
}
