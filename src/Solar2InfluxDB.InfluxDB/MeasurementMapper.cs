using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Solar2InfluxDB.Model;
using System;

namespace Solar2InfluxDB.InfluxDB
{
    public static class MeasurementMapper
    {
        public static PointData ToPointData<TValue>(this Measurement<TValue> measurement)
        {
            var point = PointData
                .Measurement(measurement.Name)
                .Timestamp(measurement.DateTime, WritePrecision.Ns);

            switch (measurement.Value)
            {
                case bool boolValue:
                    point = point.Field("value", boolValue);
                    break;
                case byte byteValue:
                    point = point.Field("value", byteValue);
                    break;
                case decimal decimalValue:
                    point = point.Field("value", decimalValue);
                    break;
                case double doubleValue:
                    point = point.Field("value", doubleValue);
                    break;
                case float floatValue:
                    point = point.Field("value", floatValue);
                    break;
                case int intValue:
                    point = point.Field("value", intValue);
                    break;
                case short shortValue:
                    point = point.Field("value", shortValue);
                    break;
                case long longValue:
                    point = point.Field("value", longValue);
                    break;
                case string stringValue:
                    point = point.Field("value", stringValue);
                    break;
                case ushort ushortValue:
                    point = point.Field("value", ushortValue);
                    break;
                case uint uintValue:
                    point = point.Field("value", uintValue);
                    break;
                case ulong ulongValue:
                    point = point.Field("value", ulongValue);
                    break;
                default:
                    throw new Exception("Type not supported");
            }

            foreach (var kv in measurement.Tags)
            {
                point = point.Tag(kv.Key, kv.Value);
            }

            return point;
        }
    }
}
