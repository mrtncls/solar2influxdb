using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solar2InfluxDB.Model
{
    public interface IMeasurementReader
    {
        Task Initialize();

        IAsyncEnumerable<MeasurementCollection> ReadMeasurementsFromDevices();
    }
}
