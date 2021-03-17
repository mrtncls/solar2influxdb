using System;
using System.Collections.Generic;

namespace Solar2InfluxDB.Model
{
    public abstract class Measurement
    {
        public Measurement(string name, params (string name, string value)[] tags)
        {
            Name = name;
            Tags = tags;
            DateTime = DateTime.UtcNow;
        }

        public string Name { get; }

        public (string name, string value)[] Tags { get; }

        public DateTime DateTime { get; } = DateTime.UtcNow;
    }

    public abstract class Measurement<TValue> : Measurement where TValue : struct
    {
        protected Measurement(string name, TValue value, params (string name, string value)[] tags) 
            : base(name, tags)
        {
            Value = value;
        }

        public TValue Value { get; }

        public override bool Equals(object obj)
        {
            var other = obj as Measurement<TValue>;

            return other != null
                && other.Value.Equals(Value)
                && TagsAreEqual(other.Tags);
        }

        private bool TagsAreEqual((string name, string value)[] otherTags)
        {
            return Tags.Length == otherTags.Length
                && new HashSet<(string name, string value)>(Tags).SetEquals(otherTags);
        }

        public override int GetHashCode()
        {
            int hashCode = 446760336;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<(string name, string value)[]>.Default.GetHashCode(Tags);
            hashCode = hashCode * -1521134295 + DateTime.GetHashCode();
            hashCode = hashCode * -1521134295 + Value.GetHashCode();
            return hashCode;
        }
    }

    public class DoubleMeasurement : Measurement<double>
    {
        public DoubleMeasurement(string name, double value, params (string name, string value)[] tags) 
            : base(name, value, tags)
        {
        }
    }

    public class IntegerMeasurement : Measurement<int>
    {
        public IntegerMeasurement(string name, int value, params (string name, string value)[] tags) 
            : base(name, value, tags)
        {
        }
    }
}
