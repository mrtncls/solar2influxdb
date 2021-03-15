using System;
using System.Collections.Generic;

namespace Solar2InfluxDB.Model
{
    public class Measurement<TValue>
    {
        public Measurement()
        {
            Tags = new Dictionary<string, string>();
            DateTime = DateTime.UtcNow;
        }

        public string Name { get; set; }

        public TValue Value { get; set; }

        public Dictionary<string, string> Tags { get; }

        public DateTime DateTime { get; }
    }
}
