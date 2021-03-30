using Solar2InfluxDB.Model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000.Parameters
{
    internal static class Custom
    {
        public static Task<MeasurementCollection> GetCustomMeasurements(this HuaweiSun2000Client client, CustomParameter[] config)
        {
            var measurements = config
                .Select(p =>
                {
                    try
                    {
                        switch (p.Type)
                        {
                            case ParameterType.Short:
                                return new IntegerMeasurement(p.Name, client.GetShort(p.Address));
                            case ParameterType.UnsignedShort:
                                return new UnsignedShortMeasurement(p.Name, client.GetUnsignedShort(p.Address));
                            case ParameterType.Integer:
                                return new IntegerMeasurement(p.Name, client.GetInteger(p.Address));
                            case ParameterType.UnsignedInteger:
                                return new UnsignedIntegerMeasurement(p.Name, client.GetUnsignedInteger(p.Address)) as Measurement;
                            default:
                                throw new NotImplementedException($"{p.Type} not implemented");
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Reading custom parameter '{p.Name}' failed", e);
                    }
                })
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "Custom",
                    measurements,
                    ("Hostname", client.Hostname)));
        }
    }
}
