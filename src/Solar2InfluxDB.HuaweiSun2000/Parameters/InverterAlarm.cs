using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000.Parameters
{
    internal static class InverterAlarm
    {
        private delegate Measurement GetAlarm(Lazy<ushort> alarm1, Lazy<ushort> alarm2, Lazy<ushort> alarm3, string name);

        private static Dictionary<string, GetAlarm> Parameters = new Dictionary<string, GetAlarm>
        {
            ["Major: High String Input Voltage"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorHighStringInputVoltage)),
            ["Major: DC Arc Fault"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorDCArcFault)),
            ["Major: String Reverse Connection"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorStringReverseConnection)),
            ["Warning: String Current Backfeed"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.WarningStringCurrentBackfeed)),
            ["Warning: Abnormal String Power"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.WarningAbnormalStringPower)),
            ["Major: AFCI Self-Check Fail"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorAFCISelfCheckFail)),
            ["Major: Phase Wire Short-Circuited to PE"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorPhaseWireShortCircuitedToPE)),
            ["Major: Grid Loss"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorGridLoss)),
            ["Major: Grid Undervoltage"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorGridUndervoltage)),
            ["Major: Grid Overvoltage"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorGridOvervoltage)),
            ["Major: Grid Volt. Imbalance"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorGridVoltImbalance)),
            ["Major: Grid Overfrequency"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorGridOverfrequency)),
            ["Major: Grid Underfrequency"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorGridUnderfrequency)),
            ["Major: Unstable Grid Frequency"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorUnstableGridFrequency)),
            ["Major: Output Overcurrent"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorOutputOvercurrent)),
            ["Major: Output DC Component Overhigh"] = (alarm1, _, __, n) => new BooleanMeasurement(n, alarm1.Value.IsFlagSet(Alarm1Flags.MajorOutputDCComponentOverhigh)),
            ["Major: Abnormal Residual Current"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorAbnormalResidualCurrent)),
            ["Major: Abnormal Grounding"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorAbnormalGrounding)),
            ["Major: Low Insulation Resistance"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorLowInsulationResistance)),
            ["Minor: Overtemperature"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MinorOvertemperature)),
            ["Major: Device Fault"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorDeviceFault)),
            ["Minor: Upgrade Failed or Version Mismatch"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MinorUpgradeFailedOrVersionMismatch)),
            ["Warning: License Expired"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.WarningLicenseExpired)),
            ["Minor: Faulty Monitoring Unit"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MinorFaultyMonitoringUnit)),
            ["Major: Faulty Power Collector"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorFaultyPowerCollector)),
            ["Minor: Battery abnormal"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MinorBatteryAbnormal)),
            ["Major: Active Islanding"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorActiveIslanding)),
            ["Major: Passive Islanding"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorPassiveIslanding)),
            ["Major: Transient AC Overvoltage"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorTransientACOvervoltage)),
            ["Warning: Peripheral port short circuit"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.WarningPeripheralPortShortCircuit)),
            ["Major: Churn output overload"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorChurnOutputOverload)),
            ["Major: Abnormal PV module configuration"] = (_, alarm2, __, n) => new BooleanMeasurement(n, alarm2.Value.IsFlagSet(Alarm2Flags.MajorAbnormalPVModuleConfiguration)),
            ["Warning: Optimizer fault"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.WarningOptimizerFault)),
            ["Minor: Built-in PID operation abnormal"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MinorBuiltInPIDOperationAbnormal)),
            ["Major: High input string voltage to ground"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MajorHighInputStringVoltageToGround)),
            ["Major: External Fan Abnormal"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MajorExternalFanAbnormal)),
            ["Major: Battery Reverse Connection"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MajorBatteryReverseConnection)),
            ["Major: On-grid/Off-grid controller abnormal"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MajorOnGridOffGridControllerAbnormal)),
            ["Warning: PV String Loss"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.WarningPVStringLoss)),
            ["Major: Internal Fan Abnormal"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MajorInternalFanAbnormal)),
            ["Major: DC Protection Unit Abnormal"] = (_, __, alarm3, n) => new BooleanMeasurement(n, alarm3.Value.IsFlagSet(Alarm3Flags.MajorDCProtectionUnitAbnormal)),
        };

        public static Task<MeasurementCollection> GetInverterAlarmMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var alarm1 = new Lazy<ushort>(() => client.GetUnsignedShort(32008));
            var alarm2 = new Lazy<ushort>(() => client.GetUnsignedShort(32009));
            var alarm3 = new Lazy<ushort>(() => client.GetUnsignedShort(32010));

            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .Select(p => Parameters[p](alarm1, alarm2, alarm3, p))
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "InverterAlarm",
                    measurements,
                    ("Hostname", client.Hostname),
                    ("Model", client.Model),
                    ("Serial number", client.SerialNumber)));
        }
    }

    internal static class AlarmFlagExtensions
    {
        public static bool IsFlagSet(this ushort value, Alarm1Flags flag) => (value & (ushort)flag) == (ushort)flag;
        public static bool IsFlagSet(this ushort value, Alarm2Flags flag) => (value & (ushort)flag) == (ushort)flag;
        public static bool IsFlagSet(this ushort value, Alarm3Flags flag) => (value & (ushort)flag) == (ushort)flag;
    }

    [Flags]
    internal enum Alarm1Flags
    {
        MajorHighStringInputVoltage = 0b0000_0000_0000_0001,
        MajorDCArcFault = 0b0000_0000_0000_0010,
        MajorStringReverseConnection = 0b0000_0000_0000_0100,
        WarningStringCurrentBackfeed = 0b0000_0000_0000_1000,
        WarningAbnormalStringPower = 0b0000_0000_0001_0000,
        MajorAFCISelfCheckFail = 0b0000_0000_0010_0000,
        MajorPhaseWireShortCircuitedToPE = 0b0000_0000_0100_0000,
        MajorGridLoss = 0b0000_0000_1000_0000,
        MajorGridUndervoltage = 0b0000_0001_0000_0000,
        MajorGridOvervoltage = 0b0000_0010_0000_0000,
        MajorGridVoltImbalance = 0b0000_0100_0000_0000,
        MajorGridOverfrequency = 0b0000_1000_0000_0000,
        MajorGridUnderfrequency = 0b0001_0000_0000_0000,
        MajorUnstableGridFrequency = 0b0010_0000_0000_0000,
        MajorOutputOvercurrent = 0b0100_0000_0000_0000,
        MajorOutputDCComponentOverhigh = 0b1000_0000_0000_0000,
    }

    [Flags]
    internal enum Alarm2Flags
    {
        MajorAbnormalResidualCurrent = 0b0000_0000_0000_0001,
        MajorAbnormalGrounding = 0b0000_0000_0000_0010,
        MajorLowInsulationResistance = 0b0000_0000_0000_0100,
        MinorOvertemperature = 0b0000_0000_0000_1000,
        MajorDeviceFault = 0b0000_0000_0001_0000,
        MinorUpgradeFailedOrVersionMismatch = 0b0000_0000_0010_0000,
        WarningLicenseExpired = 0b0000_0000_0100_0000,
        MinorFaultyMonitoringUnit = 0b0000_0000_1000_0000,
        MajorFaultyPowerCollector = 0b0000_0001_0000_0000,
        MinorBatteryAbnormal = 0b0000_0010_0000_0000,
        MajorActiveIslanding = 0b0000_0100_0000_0000,
        MajorPassiveIslanding = 0b0000_1000_0000_0000,
        MajorTransientACOvervoltage = 0b0001_0000_0000_0000,
        WarningPeripheralPortShortCircuit = 0b0010_0000_0000_0000,
        MajorChurnOutputOverload = 0b0100_0000_0000_0000,
        MajorAbnormalPVModuleConfiguration = 0b1000_0000_0000_0000,
    }

    [Flags]
    internal enum Alarm3Flags
    {
        WarningOptimizerFault = 0b0000_0000_0000_0001,
        MinorBuiltInPIDOperationAbnormal = 0b0000_0000_0000_0010,
        MajorHighInputStringVoltageToGround = 0b0000_0000_0000_0100,
        MajorExternalFanAbnormal = 0b0000_0000_0000_1000,
        MajorBatteryReverseConnection = 0b0000_0000_0001_0000,
        MajorOnGridOffGridControllerAbnormal = 0b0000_0000_0010_0000,
        WarningPVStringLoss = 0b0000_0000_0100_0000,
        MajorInternalFanAbnormal = 0b0000_0000_1000_0000,
        MajorDCProtectionUnitAbnormal = 0b0000_0001_0000_0000,
    }
}
