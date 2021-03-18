//using FluentAssertions;
//using Solar2InfluxDB.Model;
//using System.Collections.Generic;
//using Xunit;

//namespace Solar2InfluxDB.Worker.Tests
//{
//    public class MeasurementTestDoubles
//    {
//        public static IEnumerable<object[]> MeasurementsNotChanged => new List<object[]>()
//        {
//            new[] 
//            { 
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "XYZ")),
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "XYZ")) 
//            },
//            new[] 
//            { 
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "XYZ")),
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "XYZ")) 
//            }
//        };

//        public static IEnumerable<object[]> MeasurementsChanged => new List<object[]>()
//        {
//            new[] 
//            { 
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "XYZ")),
//                new DoubleMeasurement("double", 2.789d, ("host", "localhost"), ("model", "XYZ")) 
//            },
//            new[] 
//            { 
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "XYZ")),
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "XYZ"), ("region", "seaside")) 
//            },
//            new[] 
//            { 
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "123")),
//                new DoubleMeasurement("double", 0.154d, ("host", "localhost"), ("model", "XYZ")) 
//            },
//            new[]
//            {
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "XYZ")),
//                new IntegerMeasurement("int", 84415, ("host", "localhost"), ("model", "XYZ"))
//            },
//            new[]
//            {
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "XYZ")),
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "TYF"))
//            },
//            new[]
//            {
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "XYZ")),
//                new IntegerMeasurement("int", 687, ("host", "localhost"), ("model", "XYZ"), ("region", "seaside"))
//            }
//        };
//    }

//    public class MeasurementChangedTrackerTests
//    {
//        public MeasurementChangedTrackerTests()
//        {
//            ChangedTracker = new MeasurementChangedTracker();
//        }

//        private MeasurementChangedTracker ChangedTracker { get; }

//        [Theory]
//        [MemberData(nameof(MeasurementTestDoubles.MeasurementsNotChanged), MemberType = typeof(MeasurementTestDoubles))]
//        public void GivenNewMeasurement_WhenCheckingIsChanged_ThenReturnsTrue(Measurement init, Measurement _)
//        {
//            ChangedTracker.IsMeasurementChanged(init)
//                .Should().BeTrue();
//        }

//        [Theory]
//        [MemberData(nameof(MeasurementTestDoubles.MeasurementsChanged), MemberType = typeof(MeasurementTestDoubles))]
//        public void GivenMeasurementValueChanged_WhenCheckingIsChanged_ThenReturnsTrue(Measurement init, Measurement update)
//        {
//            ChangedTracker.IsMeasurementChanged(init);

//            ChangedTracker.IsMeasurementChanged(update)
//                .Should().BeTrue();
//        }

//        [Theory]
//        [MemberData(nameof(MeasurementTestDoubles.MeasurementsNotChanged), MemberType = typeof(MeasurementTestDoubles))]
//        public void GivenMeasurementValueNotChanged_WhenCheckingIsChanged_ThenReturnsFalse(Measurement init, Measurement update)
//        {
//            ChangedTracker.IsMeasurementChanged(init);

//            ChangedTracker.IsMeasurementChanged(update)
//                .Should().BeFalse();
//        }
//    }
//}
