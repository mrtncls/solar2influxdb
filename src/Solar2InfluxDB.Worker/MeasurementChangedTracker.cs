using System.Collections.Generic;
using Solar2InfluxDB.Model;

namespace Solar2InfluxDB.Worker
{
    public class MeasurementChangedTracker
    {
        Dictionary<string, Measurement> lastMeasured = new Dictionary<string, Measurement>();

        public void CalculateAlreadyStored(MeasurementCollection measurements)
        {
            foreach (var measurement in measurements)
            {
                string key = $"{measurements.Name}.{measurement.Name}";

                Process(key, measurement);
            }
        }

        private void Process(string key, Measurement measurement)
        {
            if (lastMeasured.ContainsKey(key) && !lastMeasured[key].IsDifferent(measurement))
            {
                measurement.MarkAsAlreadyStored();
            }

            lastMeasured[key] = measurement;
        }
    }
}