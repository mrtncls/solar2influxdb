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

            // Undocumented:
            ["Undocumented 1.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37003)) },
            ["Undocumented 1.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37003)) },
            ["Undocumented 1.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37004)) },
            ["Undocumented 1.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37004)) },
            ["Undocumented 2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37005)) },
            ["Undocumented 2 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37005)) },
            ["Undocumented 2.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37005)) },
            ["Undocumented 2.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37005)) },
            ["Undocumented 2.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37006)) },
            ["Undocumented 2.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37006)) },
            ["Undocumented 3"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37007)) },
            ["Undocumented 3 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37007)) },
            ["Undocumented 3.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37007)) },
            ["Undocumented 3.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37007)) },
            ["Undocumented 3.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37008)) },
            ["Undocumented 3.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37008)) },
            ["Undocumented 4"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37009)) },
            ["Undocumented 4 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37009)) },
            ["Undocumented 4.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37009)) },
            ["Undocumented 4.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37009)) },
            ["Undocumented 4.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37010)) },
            ["Undocumented 4.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37010)) },
            ["Undocumented 5"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37011)) },
            ["Undocumented 5 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37011)) },
            ["Undocumented 5.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37011)) },
            ["Undocumented 5.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37011)) },
            ["Undocumented 5.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37012)) },
            ["Undocumented 5.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37012)) },
            ["Undocumented 6"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37013)) },
            ["Undocumented 6 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37013)) },
            ["Undocumented 6.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37013)) },
            ["Undocumented 6.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37013)) },
            ["Undocumented 6.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37012)) },
            ["Undocumented 6.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37012)) },
            ["Undocumented 7"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37019)) },
            ["Undocumented 7 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37019)) },
            ["Undocumented 7.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37019)) },
            ["Undocumented 7.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37019)) },
            ["Undocumented 7.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37020)) },
            ["Undocumented 7.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37020)) },
            ["Undocumented 8"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37021)) },
            ["Undocumented 8 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37021)) },
            ["Undocumented 8.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37021)) },
            ["Undocumented 8.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37021)) },
            ["Undocumented 8.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37022)) },
            ["Undocumented 8.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37022)) },
            ["Undocumented 9"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37022)) },
            ["Undocumented 9 u"] = (c, n) => new[] { new UnsignedIntegerMeasurement(n, c.GetUnsignedInteger(37022)) },
            ["Undocumented 9.1"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37022)) },
            ["Undocumented 9.1 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37022)) },
            ["Undocumented 9.2"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetShort(37023)) },
            ["Undocumented 9.2 u"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetUnsignedShort(37023)) },
            //["Undocumented u 10"] = (c, n) => new[] { nUnsignedIntegerMeasurement(n, c.GetUnsignedIntegerGetShort(37023)) },
            //["Undocumented 11"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37025)) },
            //["Undocumented 12"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37027)) },
            //["Undocumented 13"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37029)) },
            //["Undocumented 14"] = (c, n) => new[] { new IntegerMeasurement(n, c.GetInteger(37031)) },


            // Static on my setup:
            ["Mode"] = (c, n) => new[] { new StringMeasurement(n, ((BatteryWorkingMode)c.GetUnsignedShort(47004)).ToString()) },
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
