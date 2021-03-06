using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000.Parameters
{
    internal static class PVStrings
    {
        private static Dictionary<string, Func<HuaweiSun2000Client, string, IEnumerable<Measurement>>> Parameters = new Dictionary<string, Func<HuaweiSun2000Client, string, IEnumerable<Measurement>>>
        {
            ["Voltage of all PV strings [V]"] = GetVoltageOfAllPVStrings,
            ["Current of all PV strings [A]"] = GetCurrentOfAllPVStrings,
        };

        private static IEnumerable<Measurement> GetVoltageOfAllPVStrings(HuaweiSun2000Client client, string _)
        {
            var measurements = new List<Measurement>();
            var pvStringsCount = client.GetNumberOfPVStrings();

            for (int i = 1; i <= pvStringsCount; i++)
            {
                measurements.Add(new DoubleMeasurement($"PV string {i} voltage [V]", client.GetPVVoltage(i)));
            }

            return measurements;
        }

        private static IEnumerable<Measurement> GetCurrentOfAllPVStrings(HuaweiSun2000Client client, string _)
        {
            var measurements = new List<Measurement>();
            var pvStringsCount = client.GetNumberOfPVStrings();

            for (int i = 1; i <= pvStringsCount; i++)
            {
                measurements.Add(new DoubleMeasurement($"PV string {i} current [A]", client.GetPVCurrent(i)));
            }

            return measurements;
        }

        public static Task<MeasurementCollection> GetPVStringsMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .SelectMany(p =>
                {
                    try
                    {
                        return Parameters[p](client, p);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Reading PV string parameter '{p}' failed", e);
                    }
                })
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "PVStrings",
                    measurements,
                    ("Hostname", client.Hostname)));
        }

        private static ushort GetNumberOfPVStrings(this HuaweiSun2000Client client) => client.GetUnsignedShort(30071);
        private static double GetPVVoltage(this HuaweiSun2000Client client, int PVString) => (double)client.GetShort(32014 + 2 * PVString) / 10;
        private static double GetPVCurrent(this HuaweiSun2000Client client, int PVString) => (double)client.GetShort(32015 + 2 * PVString) / 100;
    }
}
