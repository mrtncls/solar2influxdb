using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Solar2InfluxDB.Model
{
    public class MeasurementCollection : ReadOnlyCollection<Measurement>
    {
        public MeasurementCollection(string name, IList<Measurement> list, params (string name, string value)[] tags)
            : base(list)
        {
            Name = name;
            Tags = tags;
            DateTime = DateTime.UtcNow;
        }

        public string Name { get; }

        public (string name, string value)[] Tags { get; }

        public DateTime DateTime { get; }
    }
}
