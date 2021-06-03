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
            ["Device state"] = (_, __, ___, deviceState, n) => new StringMeasurement(n, deviceState.Value.ToStateString(DeviceStates)),
        };

        private static Dictionary<ushort, string> DeviceStates = new Dictionary<ushort, string>
        {
            [0x0000] = "Standby: initializing",
            [0x0001] = "Standby: detecting insulation resistance",
            [0x0002] = "Standby: detecting irradiation",
            [0x0003] = "Standby: grid detecting",
            [0x0100] = "Starting",
            [0x0200] = "Running",
            [0x0201] = "Power limited",
            [0x0202] = "Self-derating",
            [0x0300] = "Shutdown: fault",
            [0x0301] = "Shutdown: command",
            [0x0302] = "Shutdown: OVGR",
            [0x0303] = "Shutdown: communication disconnected",
            [0x0304] = "Shutdown: power limited",
            [0x0305] = "Shutdown: manual startup required",
            [0x0306] = "Shutdown: DC switches disconnected",
            [0x0307] = "Shutdown: rapid cutoff",
            [0x0308] = "Shutdown: input underpower",
            [0x0401] = "Grid scheduling: cos F-P curve",
            [0x0402] = "Grid scheduling: Q-U curve",
            [0x0403] = "Grid scheduling: PF-U curve",
            [0x0404] = "Grid scheduling: dry contact",
            [0x0405] = "Grid scheduling: Q-P curve",
            [0x0500] = "Spot-check ready",
            [0x0501] = "Spot-checking",
            [0x0600] = "Inspecting",
            [0x0700] = "AFCI self check",
            [0x0800] = "I-V scanning",
            [0x0900] = "DC input detection",
            [0x0a00] = "Charging",
            [0xa000] = "Standby: no irradiation",
        };

        public static Task<MeasurementCollection> GetInverterStateMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var state1 = new Lazy<ushort>(() => client.GetUnsignedShort(32000));
            var state2 = new Lazy<ushort>(() => client.GetUnsignedShort(32002));
            var state3 = new Lazy<uint>(() => client.GetUnsignedInteger(32003));
            var deviceStatus = new Lazy<ushort>(() => client.GetUnsignedShort(32089));

            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .Select(p =>
                {
                    try
                    {
                        return Parameters[p](state1, state2, state3, deviceStatus, p);
                    }
                    catch (Exception e)
                    {
                        throw new Exception($"Reading inverter state '{p}' failed", e);
                    }
                }).ToArray();

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
        public static bool IsFlagSet(this uint value, State3Flags flag) => (value & (uint)flag) == (uint)flag;
        public static string ToStateString(this ushort value, Dictionary<ushort, string> DeviceStates)
        {
            if (!DeviceStates.ContainsKey(value))
            {
                return "Unknown";
            }

            return DeviceStates[value];
        }
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
}
