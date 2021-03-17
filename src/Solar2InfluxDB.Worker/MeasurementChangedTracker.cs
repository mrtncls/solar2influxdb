using System.Collections.Generic;
using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.Worker
{
    public class MeasurementChangedTracker
    {
        Dictionary<string, Measurement> lastMeasured = new Dictionary<string, Measurement>();

        public bool IsMeasurementChanged(Measurement measuremnt)
        {
            bool changed = true;

            if (lastMeasured.ContainsKey(measuremnt.Name)
                && lastMeasured[measuremnt.Name].Equals(measuremnt))
            {
                changed = false;
            }

            lastMeasured[measuremnt.Name] = measuremnt;

            return changed;
        }
    }
}
