using System.Threading.Tasks;

namespace Solar2InfluxDB.Model
{
    public interface IMeasurementWriter
    {
        Task Initialize();

        Task Write(MeasurementCollection measurements);

    }
}
