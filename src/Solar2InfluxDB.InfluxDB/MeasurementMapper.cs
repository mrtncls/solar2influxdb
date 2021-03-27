using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using Solar2InfluxDB.Model;
using System;

namespace Solar2InfluxDB.InfluxDB
{
    public static class MeasurementMapper
    {
        public static PointData ToPointData(this MeasurementCollection measurements)
        {
            var point = PointData
                .Measurement(measurements.Name)
                .Timestamp(measurements.DateTime, WritePrecision.Ns);

            foreach (var measurement in measurements)
            {
                switch (measurement)
                {
                    case DoubleMeasurement doubleMeasurement:
                        point = point.Field(doubleMeasurement.Name, doubleMeasurement.Value);
                        break;
                    case IntegerMeasurement integerMeasurement:
                        point = point.Field(integerMeasurement.Name, integerMeasurement.Value);
                        break;
                    case UnsignedShortMeasurement unsignedShortMeasurement:
                        point = point.Field(unsignedShortMeasurement.Name, unsignedShortMeasurement.Value);
                        break;
                    case UnsignedIntegerMeasurement unsignedIntegerMeasurement:
                        point = point.Field(unsignedIntegerMeasurement.Name, unsignedIntegerMeasurement.Value);
                        break;
                    case BooleanMeasurement booleanMeasurement:
                        point = point.Field(booleanMeasurement.Name, booleanMeasurement.Value);
                        break;
                    case StringMeasurement stringMeasurement:
                        point = point.Field(stringMeasurement.Name, stringMeasurement.Value);
                        break;
                    default:
                        throw new NotImplementedException($"Measurement mapping for {measurement.GetType().Name} not implemented");
                }
            }

            foreach (var tag in measurements.Tags)
            {
                point = point.Tag(tag.name, tag.value);
            }

            return point;
        }
    }
}
