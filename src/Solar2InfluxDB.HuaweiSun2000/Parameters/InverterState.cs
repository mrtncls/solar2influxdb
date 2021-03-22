﻿using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000
{
    internal static class InverterState
    {
        public static Dictionary<string, Func<Lazy<ushort>, Lazy<ushort>, Lazy<uint>, string, Measurement>> Parameters = new Dictionary<string, Func<Lazy<ushort>, Lazy<ushort>, Lazy<uint>, string, Measurement>>
        {
            ["State 1: Standby"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.Standby)),
            ["State 1: Grid connected"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnected)),
            ["State 1: Grid connected normally"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnectedNormally)),
            ["State 1: Grid connection derated due to power rationing"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnectionWithDeratingDueToPowerRationing)),
            ["State 1: Grid connection derated due to internal cause"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.GridConnectionWithFeratingDueToInternalCausesOfTheSolarInverter)),
            ["State 1: Stop normal"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.NormalStop)),
            ["State 1: Stop due to faults"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.StopDueToFaults)),
            ["State 1: Stop due to power rationing"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.StopDueToPowerRationing)),
            ["State 1: Shutdown"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.Shutdown)),
            ["State 1: Sport check"] = (state1, _, __, n) => new BooleanMeasurement(n, state1.Value.IsFlagSet(State1Flags.SpotCheck)),
            ["State 2: Locked"] = (_, state2, __, n) => new BooleanMeasurement(n, state2.Value.IsFlagSet(State2Flags.Locked)),
            ["State 2: PV connected"] = (_, state2, __, n) => new BooleanMeasurement(n, state2.Value.IsFlagSet(State2Flags.PVConnected)),
            ["State 2: DSP data"] = (_, state2, __, n) => new BooleanMeasurement(n, state2.Value.IsFlagSet(State2Flags.DSPData)),
            ["State 3: Off grid"] = (_, __, state3, n) => new BooleanMeasurement(n, state3.Value.IsFlagSet(State3Flags.OffGrid)),
            ["State 3: Off grid switch"] = (_, __, state3, n) => new BooleanMeasurement(n, state3.Value.IsFlagSet(State3Flags.OffGridSwitch)),
        };


        public static Task<MeasurementCollection> GetInverterStateMeasurements(this HuaweiSun2000Client client, ParameterConfig config)
        {
            var state1 = new Lazy<ushort>(() => client.GetUnsignedShort(32000));
            var state2 = new Lazy<ushort>(() => client.GetUnsignedShort(32002));
            var state3 = new Lazy<uint>(() => client.GetUnsignedInteger(32003));

            var measurements = config.ParametersToRead
                .Where(p => Parameters.ContainsKey(p))
                .Select(p => Parameters[p](state1, state2, state3, p))
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

    public static class UnsignedShortExtensions
    {
        public static bool IsFlagSet(this ushort value, State1Flags flag)
        {
            return (value & (ushort)flag) == (ushort)flag;
        }

        public static bool IsFlagSet(this ushort value, State2Flags flag)
        {
            return (value & (ushort)flag) == (ushort)flag;
        }
    }
    
    public static class UnsignedIntegerExtensions
    {
        public static bool IsFlagSet(this uint value, State3Flags flag)
        {
            return (value & (uint)flag) == (uint)flag;
        }
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
    public enum State1Flags
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
    public enum State2Flags
    {
        Locked = 0b0000_0000_0000_0001,
        PVConnected = 0b0000_0000_0000_0010,
        DSPData = 0b0000_0000_0000_0100,
    }

    [Flags]
    public enum State3Flags
    {
        OffGrid = 0b0000_0000_0000_0000_0000_0000_0000_0001,
        OffGridSwitch = 0b0000_0000_0000_0000_0000_0000_0000_0010,
    }

    //    0x0000: 'Standby: initializing',
    //0x0001: 'Standby: detecting insulation resistance',
    //0x0002: 'Standby: detecting irradiation',
    //0x0003: 'Standby: grid detecting',
    //0x0100: 'Starting',
    //0x0200: 'On-grid (Off-grid mode: running)',
    //0x0201: 'Grid connection: power limited (Off-grid mode: running: power limited)',
    //0x0202: 'Grid connection: self-derating (Off-grid mode: running: self-derating)',
    //0x0300: 'Shutdown: fault',
    //0x0301: 'Shutdown: command',
    //0x0302: 'Shutdown: OVGR',
    //0x0303: 'Shutdown: communication disconnected',
    //0x0304: 'Shutdown: power limited',
    //0x0305: 'Shutdown: manual startup required',
    //0x0306: 'Shutdown: DC switches disconnected',
    //0x0307: 'Shutdown: rapid cutoff',
    //0x0308: 'Shutdown: input underpower',
    //0x0401: 'Grid scheduling: cos F-P curve',
    //0x0402: 'Grid scheduling: Q-U curve',
    //0x0403: 'Grid scheduling: PF-U curve',
    //0x0404: 'Grid scheduling: dry contact',
    //0x0405: 'Grid scheduling: Q-P curve',
    //0x0500: 'Spot-check ready',
    //0x0501: 'Spot-checking',
    //0x0600: 'Inspecting',
    //0x0700: 'AFCI self check',
    //0x0800: 'I-V scanning',
    //0x0900: 'DC input detection',
    //0x0a00: 'Running: off-grid charging',
    //0xa000: 'Standby: no irradiation'
}
