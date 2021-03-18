using System.Threading.Tasks;

namespace Solar2InfluxDB.Model
{
    public interface IMeasurementReader
    {
        Task Initialize();

        Task<MeasurementCollection[]> ReadMeasurementsFromDevices();
    }
}
