using Solar2InfluxDB.Model;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class PowerMeter
    {
        public static DoubleMeasurement GetPowerMeterVoltagePhaseA(this HuaweiSun2000Client client) => client.GetMeasurement("Voltage phase A [V]", (double)client.GetInteger(37101) / 10);
        public static DoubleMeasurement GetPowerMeterVoltagePhaseB(this HuaweiSun2000Client client) => client.GetMeasurement("Voltage phase B [V]", (double)client.GetInteger(37103) / 10);
        public static DoubleMeasurement GetPowerMeterVoltagePhaseC(this HuaweiSun2000Client client) => client.GetMeasurement("Voltage phase C [V]", (double)client.GetInteger(37105) / 10);
        public static DoubleMeasurement GetPowerMeterCurrentPhaseA(this HuaweiSun2000Client client) => client.GetMeasurement("Current phase A [A]", (double)client.GetInteger(37107) / 100);
        public static DoubleMeasurement GetPowerMeterCurrentPhaseB(this HuaweiSun2000Client client) => client.GetMeasurement("Current phase B [A]", (double)client.GetInteger(37109) / 100);
        public static DoubleMeasurement GetPowerMeterCurrentPhaseC(this HuaweiSun2000Client client) => client.GetMeasurement("Current phase C [A]", (double)client.GetInteger(37111) / 100);
        public static IntegerMeasurement GetPowerMeterPower(this HuaweiSun2000Client client) => client.GetMeasurement("Power [W]", -client.GetInteger(37113));
        public static IntegerMeasurement GetPowerMeterReactivePower(this HuaweiSun2000Client client) => client.GetMeasurement("Reactive power [VAR]", client.GetInteger(37115));
        public static DoubleMeasurement GetPowerMeterPowerFactor(this HuaweiSun2000Client client) => client.GetMeasurement("Power factor", (double)client.GetShort(37117) / 1000);
        public static DoubleMeasurement GetPowerMeterFrequency(this HuaweiSun2000Client client) => client.GetMeasurement("Frequency [Hz]", (double)client.GetShort(37118) / 100);
        public static DoubleMeasurement GetPowerMeterExportedEnergy(this HuaweiSun2000Client client) => client.GetMeasurement("Exported energy [kWh]", (double)client.GetInteger(37119) / 100);
        public static DoubleMeasurement GetPowerMeterAccumulatedEnergy(this HuaweiSun2000Client client) => client.GetMeasurement("Accumulated energy [kWh]", (double)client.GetUnsignedInteger(37121) / 100);
        public static DoubleMeasurement GetPowerMeterVoltageAB(this HuaweiSun2000Client client) => client.GetMeasurement("Voltage AB [V]", (double)client.GetInteger(37126) / 10);
        public static DoubleMeasurement GetPowerMeterVoltageBC(this HuaweiSun2000Client client) => client.GetMeasurement("Voltage BC [V]", (double)client.GetInteger(37128) / 10);
        public static DoubleMeasurement GetPowerMeterVoltageCA(this HuaweiSun2000Client client) => client.GetMeasurement("Voltage Ca [V]", (double)client.GetInteger(37130) / 10);
        public static IntegerMeasurement GetPowerMeterPowerA(this HuaweiSun2000Client client) => client.GetMeasurement("Power A [W]", -client.GetInteger(37132));
        public static IntegerMeasurement GetPowerMeterPowerB(this HuaweiSun2000Client client) => client.GetMeasurement("Power B [W]", -client.GetInteger(37134));
        public static IntegerMeasurement GetPowerMeterPowerC(this HuaweiSun2000Client client) => client.GetMeasurement("Power C [W]", -client.GetInteger(37136));

        public static Task<MeasurementCollection> GetPowerMeterMeasurements(this HuaweiSun2000Client client, string hostname)
        {
            return Task.FromResult(
                new MeasurementCollection(
                    "Power meter",
                    new Measurement[]
                    {
                        client.GetPowerMeterVoltagePhaseA(),
                        client.GetPowerMeterVoltagePhaseB(),
                        client.GetPowerMeterVoltagePhaseC(),
                        client.GetPowerMeterCurrentPhaseA(),
                        client.GetPowerMeterCurrentPhaseB(),
                        client.GetPowerMeterCurrentPhaseC(),
                        client.GetPowerMeterPower(),
                        client.GetPowerMeterReactivePower(),
                        client.GetPowerMeterPowerFactor(),
                        client.GetPowerMeterFrequency(),
                        client.GetPowerMeterExportedEnergy(),
                        client.GetPowerMeterAccumulatedEnergy(),
                        client.GetPowerMeterVoltageAB(),
                        client.GetPowerMeterVoltageBC(),
                        client.GetPowerMeterVoltageCA(),
                        client.GetPowerMeterPowerA(),
                        client.GetPowerMeterPowerB(),
                        client.GetPowerMeterPowerC()
                    },
                    ("hostname", hostname)));
        }
    }
}
