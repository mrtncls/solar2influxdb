using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000.Parameters
{
    internal static class InverterState
    {
        private delegate Measurement GetState(Lazy<ushort> state1, Lazy<ushort> state2, Lazy<uint> state3, Lazy<ushort> deviceState, string name);

        private static Dictionary<string, GetState> Parameters = new Dictionary<string, GetState>
        {
            ["State 1: Standby"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.Standby)),
            ["State 1: Grid connected"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnected)),
            ["State 1: Grid connected normally"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnectedNormally)),
            ["State 1: Grid connection derated due to power rationing"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnectionWithDeratingDueToPowerRationing)),
            ["State 1: Grid connection derated due to internal cause"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnectionWithFeratingDueToInternalCausesOfTheSolarInverter)),
            ["State 1: Stop normal"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.NormalStop)),
            ["State 1: Stop due to faults"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.StopDueToFaults)),
            ["State 1: Stop due to power rationing"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.StopDueToPowerRationing)),
            ["State 1: Shutdown"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.Shutdown)),
            ["State 1: Sport check"] = (state1, _, __, ___, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.SpotCheck)),
            ["State 2: Locked"] = (_, state2, __, ___, n) => new BooleanMeasurement(n, state2.Value.IsFlagSet(State2Flags.Locked)),
            ["State 2: PV connected"] = (_, state2, __, ___, n) => new BooleanMeasurement(n, state2.Value.IsFlagSet(State2Flags.PVConnected)),
            ["State 2: DSP data"] = (_, state2, __, ___, n) => new BooleanMeasurement(n, state2.Value.IsFlagSet(State2Flags.DSPData)),
            ["State 3: Off grid"] = (_, __, state3, ___, n) => new BooleanMeasurement(n, state3.Value.IsFlagSet(State3Flags.OffGrid)),
            ["State 3: Off grid switch"] = (_, __, state3, ___, n) => new BooleanMeasurement(n, state3.Value.IsFlagSet(State3Flags.OffGridSwitch)),
            ["Device state: Standby: initializing"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.StandbyInitializing)),
            ["Device state: Standby: detecting insulation resistance"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.StandbyDetectingInsulationResistance)),
            ["Device state: Standby: detecting irradiation"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.StandbyDetectingIrradiation)),
            ["Device state: Standby: grid detecting"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.StandbyGridDetecting)),
            ["Device state: Starting"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.Starting)),
            ["Device state: On-grid (Off-grid mode: running)"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.OnGridOffGridModeRunning)),
            ["Device state: Grid connection: power limited (Off-grid mode: running: power limited)"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridConnectionPowerLimitedOffGridModeRunningPowerLimited)),
            ["Device state: Grid connection: self-derating (Off-grid mode: running: self-derating)"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridConnectionSelfDeratingOffGridModeRunningSelfDerating)),
            ["Device state: Shutdown: fault"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownFault)),
            ["Device state: Shutdown: command"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownCommand)),
            ["Device state: Shutdown: OVGR"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownOVGR)),
            ["Device state: Shutdown: communication disconnected"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownCommunicationDisconnected)),
            ["Device state: Shutdown: power limited"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownPowerLimited)),
            ["Device state: Shutdown: manual startup required"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownManualStartupRequired)),
            ["Device state: Shutdown: DC switches disconnected"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownDCSwitchesDisconnected)),
            ["Device state: Shutdown: rapid cutoff"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownRapidCutoff)),
            ["Device state: Shutdown: input underpower"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.ShutdownInputUnderpower)),
            ["Device state: Grid scheduling: cos F-P curve"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridSchedulingCosFPCurve)),
            ["Device state: Grid scheduling: Q-U curve"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridSchedulingQUCurve)),
            ["Device state: Grid scheduling: PF-U curve"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridSchedulingPFUCurve)),
            ["Device state: Grid scheduling: dry contact"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridSchedulingDryContact)),
            ["Device state: Grid scheduling: Q-P curve"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.GridSchedulingQPCurve)),
            ["Device state: Spot-check ready"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.SpotCheckReady)),
            ["Device state: Spot-checking"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.SpotChecking)),
            ["Device state: Inspecting"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.Inspecting)),
            ["Device state: AFCI self check"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.AFCISelfCheck)),
            ["Device state: I-V scanning"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.IVScanning)),
            ["Device state: DC input detection"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.DCInputDetection)),
            ["Device state: Running: off-grid charging"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.RunningOffGridCharging)),
            ["Device state: Standby: no irradiation"] = (_, __, ___, deviceState, n) => new BooleanMeasurement(n, deviceState.Value.IsFlagSet(DeviceStatusFlags.StandbyNoIrradiation)),
        };

        public static Task<MeasurementCollection> GetInverterStateMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var state1 = new Lazy<ushort>(() => client.GetUnsignedShort(32000));
            var state2 = new Lazy<ushort>(() => client.GetUnsignedShort(32002));
            var state3 = new Lazy<uint>(() => client.GetUnsignedInteger(32003));
            var deviceStatus = new Lazy<ushort>(() => client.GetUnsignedShort(32089));

            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .Select(p => Parameters[p](state1, state2, state3, deviceStatus, p))
                .ToArray();

            return Task.FromResult(
                new MeasurementCollection(
                    "InverterState",
                    measurements,
                    ("Hostname", client.Hostname),
                    ("Model", client.Model),
                    ("Serial number", client.SerialNumber)));
        }
    }

    internal static class StateFlagExtensions
    {
        public static bool IsFlagSet(this ushort value, State1Flags flag) => (value & (ushort)flag) == (ushort)flag;
        public static bool IsFlagSet(this ushort value, State2Flags flag) => (value & (ushort)flag) == (ushort)flag;
        public static bool IsFlagSet(this ushort value, DeviceStatusFlags flag) => (value & (ushort)flag) == (ushort)flag;
        public static bool IsFlagSet(this uint value, State3Flags flag) => (value & (uint)flag) == (uint)flag;
    }

    public enum BatteryRunningStatus
    {
        Offline = 0,
        Standby = 1,
        Running = 2,
        Fault = 3,
        SleepMode = 4,
    }

    [Flags]
    internal enum State1Flags
    {
        Standby = 0b0000_0000_0000_0001,
        GridConnected = 0b0000_0000_0000_0010,
        GridConnectedNormally = 0b0000_0000_0000_0100,
        GridConnectionWithDeratingDueToPowerRationing = 0b0000_0000_0000_1000,
        GridConnectionWithFeratingDueToInternalCausesOfTheSolarInverter = 0b0000_0000_0001_0000,
        NormalStop = 0b0000_0000_0010_0000,
        StopDueToFaults = 0b0000_0000_0100_000,
        StopDueToPowerRationing = 0b0000_0000_1000_0000,
        Shutdown = 0b0000_0001_0000_0000,
        SpotCheck = 0b0000_0010_0000_0000,
    }

    [Flags]
    internal enum State2Flags
    {
        Locked = 0b0000_0000_0000_0001,
        PVConnected = 0b0000_0000_0000_0010,
        DSPData = 0b0000_0000_0000_0100,
    }

    [Flags]
    internal enum State3Flags
    {
        OffGrid = 0b0000_0000_0000_0000_0000_0000_0000_0001,
        OffGridSwitch = 0b0000_0000_0000_0000_0000_0000_0000_0010,
    }

    [Flags]
    internal enum DeviceStatusFlags
    {
        StandbyInitializing = 0x0000,
        StandbyDetectingInsulationResistance = 0x0001,
        StandbyDetectingIrradiation = 0x0002,
        StandbyGridDetecting = 0x0003,
        Starting = 0x0100,
        OnGridOffGridModeRunning = 0x0200,
        GridConnectionPowerLimitedOffGridModeRunningPowerLimited = 0x0201,
        GridConnectionSelfDeratingOffGridModeRunningSelfDerating = 0x0202,
        ShutdownFault = 0x0300,
        ShutdownCommand = 0x0301,
        ShutdownOVGR = 0x0302,
        ShutdownCommunicationDisconnected = 0x0303,
        ShutdownPowerLimited = 0x0304,
        ShutdownManualStartupRequired = 0x0305,
        ShutdownDCSwitchesDisconnected = 0x0306,
        ShutdownRapidCutoff = 0x0307,
        ShutdownInputUnderpower = 0x0308,
        GridSchedulingCosFPCurve = 0x0401,
        GridSchedulingQUCurve = 0x0402,
        GridSchedulingPFUCurve = 0x0403,
        GridSchedulingDryContact = 0x0404,
        GridSchedulingQPCurve = 0x0405,
        SpotCheckReady = 0x0500,
        SpotChecking = 0x0501,
        Inspecting = 0x0600,
        AFCISelfCheck = 0x0700,
        IVScanning = 0x0800,
        DCInputDetection = 0x0900,
        RunningOffGridCharging = 0x0a00,
        StandbyNoIrradiation = 0xa000
    }
}
