using System.Collections.Generic;
using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.Worker
{
    public class MeasurmentChangedTracker
    {
        Dictionary<string, object> lastMeasured = new Dictionary<string, object>();

        public bool IsMeasurementChanged<TValue>(Measurement<TValue> measuremnt)
        {
            bool changed = false;

            if (!lastMeasured.ContainsKey(measuremnt.Name)
                || ((TValue)lastMeasured[measuremnt.Name]).Equals(measuremnt.Value))
            {
                changed = true;
            }

            lastMeasured[measuremnt.Name] = measuremnt.Value;

            return changed;
        }
    }
}
