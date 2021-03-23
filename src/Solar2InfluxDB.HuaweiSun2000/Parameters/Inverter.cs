using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000
{
    internal static class Inverter
    {
        private static Dictionary<string, Func<HuaweiSun2000Client, string, IEnumerable<Measurement>>> Parameters = new Dictionary<string, Func<HuaweiSun2000Client, string, IEnumerable<Measurement>>>
        {
            ["Rated power [kW]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(30073) / 1000) },
            ["Max power [kW]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(30075) / 1000) },
            ["Max apparent power [kVA]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(30077) / 1000) },
            ["Max reactive power to grid [kVar]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(30079) / 1000) },
            ["Max apparent power from grid [kVar]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(30081) / 1000) },
            ["Input power [kW]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedInteger(32064) / 1000) },
            ["Voltage AB [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32066) / 10) },
            ["Voltage BC [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32067) / 10) },
            ["Voltage CA [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32068) / 10) },
            ["Voltage A [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32069) / 10) },
            ["Voltage B [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32070) / 10) },
            ["Voltage C [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32071) / 10) },
            ["Current A [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(32072) / 1000) },
            ["Current B [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(32074) / 1000) },
            ["Current C [V]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(32076) / 1000) },
            ["Peak power today [kW]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(32078) / 1000) },
            ["Power [kW]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(32080) / 1000) },
            ["Reactive power [kVar]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetInteger(32082) / 1000) },
            ["Power factor"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetShort(32084) / 1000) },
            ["Grid frequency [Hz]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32085) / 100) },
            ["Efficiency [%]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32086) / 100) },
            ["Internal temperature [°C]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetShort(32087) / 10) },
            ["Insulation resistance [MΩ]"] = (c, n) => new[] { new DoubleMeasurement(n, (double)c.GetUnsignedShort(32088) / 1000) },
    };

        public static Task<MeasurementCollection> GetInverterMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .SelectMany(p => Parameters[p](client, p))
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "Inverter",
                    measurements,
                    ("Hostname", client.Hostname),
                    ("Model", client.Model),
                    ("Serial number", client.SerialNumber)));
        }

        public static string GetModel(this HuaweiSun2000Client client) => client.GetString(30000, 30);
        public static string GetSerialNumber(this HuaweiSun2000Client client) => client.GetString(30015, 20);
        public static string GetProductNumber(this HuaweiSun2000Client client) => client.GetString(30025, 20);
        public static ushort GetModelID(this HuaweiSun2000Client client) => client.GetUnsignedShort(30070);

        public static ushort GetNumberOfMPPTrackers(this HuaweiSun2000Client client) => client.GetUnsignedShort(30072);
        public static ushort GetNumberOfOptimizers(this HuaweiSun2000Client client) => client.GetUnsignedShort(37200);
        public static ushort GetNumberOfOnlineOptimizers(this HuaweiSun2000Client client) => client.GetUnsignedShort(37201);

        // TODO state2 (13) till alarm3 (17)

        // TODO Device status (44) to (55)

        public static BatteryRunningStatus GetBatteryRunningStatus(this HuaweiSun2000Client client) => (BatteryRunningStatus)client.GetUnsignedShort(37000);
        public static int GetBatteryChargeAndDischargePower(this HuaweiSun2000Client client) => client.GetInteger(37001);
        public static double GetBatteryCurrentDayChargeCapacity(this HuaweiSun2000Client client) => (double)client.GetUnsignedInteger(37015) / 100;
        public static double GetBatteryCurrentDayDischargeCapacity(this HuaweiSun2000Client client) => (double)client.GetUnsignedInteger(37017) / 100;

        // TODO optimizer feature data (63) to end
    }
}
