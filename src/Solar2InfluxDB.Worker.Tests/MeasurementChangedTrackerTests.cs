using FluentAssertions;
using Solar2InfluxDB.Model;
using System.Collections.Generic;
using Xunit;

namespace Solar2InfluxDB.Worker.Tests
{
    public class MeasurementTestDoubles
    {
        public static IEnumerable<object[]> MeasurementsNotChanged => new List<object[]>()
        {
            new[]
            {
                new MeasurementCollection("test", new[] { new DoubleMeasurement("double", 0.154d) }),
                new MeasurementCollection("test", new[] { new DoubleMeasurement("double", 0.154d) }),
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new IntegerMeasurement("int", 687) }),
                new MeasurementCollection("test", new[] { new IntegerMeasurement("int", 687) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new UnsignedShortMeasurement("ushort", 687) }),
                new MeasurementCollection("test", new[] { new UnsignedShortMeasurement("ushort", 687) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new UnsignedIntegerMeasurement("uint", 687u) }),
                new MeasurementCollection("test", new[] { new UnsignedIntegerMeasurement("uint", 687u) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new BooleanMeasurement("bool", true) }),
                new MeasurementCollection("test", new[] { new BooleanMeasurement("bool", true) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new StringMeasurement("string", "hello") }),
                new MeasurementCollection("test", new[] { new StringMeasurement("string", "hello") })
            }
        };

        public static IEnumerable<object[]> MeasurementsChanged => new List<object[]>()
        {
            new[]
            {
                new MeasurementCollection("test", new[] { new DoubleMeasurement("double", 0.154d) }),
                new MeasurementCollection("test", new[] { new DoubleMeasurement("double", 2.789d) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new IntegerMeasurement("int", 687) }),
                new MeasurementCollection("test", new[] { new IntegerMeasurement("int", 84415) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new UnsignedShortMeasurement("ushort", 687) }),
                new MeasurementCollection("test", new[] { new UnsignedShortMeasurement("ushort", 124) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new UnsignedIntegerMeasurement("uint", 687u) }),
                new MeasurementCollection("test", new[] { new UnsignedIntegerMeasurement("uint", 786321u) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new BooleanMeasurement("bool", true) }),
                new MeasurementCollection("test", new[] { new BooleanMeasurement("bool", false) })
            },
            new[]
            {
                new MeasurementCollection("test", new[] { new StringMeasurement("string", "hello") }),
                new MeasurementCollection("test", new[] { new StringMeasurement("string", "ola") })
            }
        };
    }

    public class MeasurementChangedTrackerTests
    {
        public MeasurementChangedTrackerTests()
        {
            ChangedTracker = new MeasurementChangedTracker();
        }

        private MeasurementChangedTracker ChangedTracker { get; }

        [Theory]
        [MemberData(nameof(MeasurementTestDoubles.MeasurementsNotChanged), MemberType = typeof(MeasurementTestDoubles))]
        public void GivenNewMeasurement_WhenCalculateAlreadyStored_ThenNoneIsAlreadyStored(MeasurementCollection init, MeasurementCollection _)
        {
            ChangedTracker.CalculateAlreadyStored(init);

            init.Should().NotContain(x => x.IsAlreadyStored);
        }

        [Theory]
        [MemberData(nameof(MeasurementTestDoubles.MeasurementsChanged), MemberType = typeof(MeasurementTestDoubles))]
        public void GivenMeasurementValueChanged_WhenCalculateAlreadyStored_ThenNoneIsAlreadyStored(MeasurementCollection init, MeasurementCollection update)
        {
            ChangedTracker.CalculateAlreadyStored(init);

            ChangedTracker.CalculateAlreadyStored(update);

            update.Should().NotContain(x => x.IsAlreadyStored);
        }

        [Theory]
        [MemberData(nameof(MeasurementTestDoubles.MeasurementsNotChanged), MemberType = typeof(MeasurementTestDoubles))]
        public void GivenMeasurementValueNotChanged_WhenCalculateAlreadyStored_ThenIsAlreadyStored(MeasurementCollection init, MeasurementCollection update)
        {
            ChangedTracker.CalculateAlreadyStored(init);

            ChangedTracker.CalculateAlreadyStored(update);

            update.Should().NotContain(x => !x.IsAlreadyStored);
        }
    }
}
