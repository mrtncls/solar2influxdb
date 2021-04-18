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
            ["Charge and discharge power [W]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37001)) },
            ["Charge power today [kWh]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(37015) / 100) },
            ["Discharge power today [kWh]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(37017) / 100) },
            ["Voltage [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(37003) / 10) }, // Not in docs
            ["Capacity [%]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(37004) / 10) }, // Not in docs

            ["Unknown parameter 37005 (constant 65536004)"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37005)) }, // Not in docs
            ["Unknown parameter 37005 (constant 1000)"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37005)) }, // Not in docs
            ["Unknown parameter 37005 (constant 4)"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37006)) }, // Not in docs
            ["Unknown parameter 37007 (constant 5000)"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37007)) }, // Not in docs
            ["Unknown parameter 37009 (constant 5000)"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37009)) }, // Not in docs
            ["Unknown parameter 37011 (constant 0)"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37011)) }, // Not in docs
            ["Unknown parameter 37013 (constant 0)"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37013)) }, // Not in docs
            ["Unknown parameter 37019 (constant 53)"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37019)) }, // Not in docs
            ["Mode"] = (c, n) => new[] { new StringMeasurement(n, ((BatteryWorkingMode)c.GetUnsignedShort(47004)).ToString()) }, // Static on my setup
            ["LCOE"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(47069) / 1000) }, // Static on my setup
            ["Maximum charge power [W]"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(47075)) }, // Static on my setup
            ["Maximum discharge power [W]"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(47077)) }, // Static on my setup
            ["Power limit of grid-tied point [W]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(47079)) }, // Static on my setup
            ["Charge cutoff capacity [%]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(47081) / 10) }, // Static on my setup
            ["Discharge cutoff capacity [%]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(47082) / 10) }, // Static on my setup
            ["Forced charging and discharging period [min]"] = (c, n) => new[] { new UnsignedShortMeasurement(n, c.GetUnsignedShort(47083)) }, // Static on my setup
            ["Forced charging and discharging power [W]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(47084)) }, // Static on my setup

            // TODO
            ["Looks similar to charge/discharge power [?]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37021)) }, // Not in docs
            ["? [?]"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37022)) }, // Not in docs
        };

        public static Task<MeasurementCollection> GetBatteryMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .SelectMany(p => 
                {
                    try
                    {
                        return Parameters[p](client, p);
                    }
                    catch(Exception e)
                    {
                        throw new Exception($"Reading battery parameter '{p}' failed", e);
                    }
                })
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
