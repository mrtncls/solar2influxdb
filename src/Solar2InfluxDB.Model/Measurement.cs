namespace Solar2InfluxDB.Model
{

    public abstract class Measurement
    {
        public Measurement(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public abstract class Measurement<TValue> : Measurement
    {
        protected Measurement(string name, TValue value)
            : base(name)
        {
            Value = value;
        }

        public TValue Value { get; }
    }

    public class DoubleMeasurement : Measurement<double>
    {
        public DoubleMeasurement(string name, double value)
            : base(name, value)
        {
        }
    }

    public class IntegerMeasurement : Measurement<int>
    {
        public IntegerMeasurement(string name, int value)
            : base(name, value)
        {
        }
    }

    public class BooleanMeasurement : Measurement<bool>
    {
        public BooleanMeasurement(string name, bool value)
            : base(name, value)
        {
        }
    }

    public class StringMeasurement : Measurement<string>
    {
        public StringMeasurement(string name, string value)
            : base(name, value)
        {
        }
    }
}
