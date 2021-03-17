using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Solar2InfluxDB.Model;
using System;

namespace Solar2InfluxDB.InfluxDB
{
    public static class MeasurementMapper
    {
        public static PointData ToPointData(this Measurement measurement)
        {
            var point = PointData
                .Measurement(measurement.Name)
                .Timestamp(measurement.DateTime, WritePrecision.Ns);

            switch (measurement)
            {
                case DoubleMeasurement doubleMeasurement:
                    point = point.Field("value", doubleMeasurement.Value);
                    break;
                case IntegerMeasurement integerMeasurement:
                    point = point.Field("value", integerMeasurement.Value);
                    break;
                default:
                    throw new NotImplementedException($"Measurement mapping for {measurement.GetType().Name} not implemented");
            }

            foreach (var tag in measurement.Tags)
            {
                point = point.Tag(tag.name, tag.value);
            }

            return point;
        }
    }
}
