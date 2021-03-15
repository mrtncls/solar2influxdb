﻿using Solar2InfluxDB.Model;
using System;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public static class HuaweiSun2000ClientGetters
    {
        public static string GetModel(this HuaweiSun2000Client client) => client.GetString(30000, 30);
        public static string GetSerialNumber(this HuaweiSun2000Client client) => client.GetString(30015, 20);
        public static string GetProductNumber(this HuaweiSun2000Client client) => client.GetString(30025, 20);
        public static ushort GetModelID(this HuaweiSun2000Client client) => client.GetUnsignedShort(30070);
        public static ushort GetNumberOfPVStrings(this HuaweiSun2000Client client) => client.GetUnsignedShort(30071);
        public static ushort GetNumberOfMPPTrackers(this HuaweiSun2000Client client) => client.GetUnsignedShort(30072);
        public static Measurement<double> GetRatedPower(this HuaweiSun2000Client client) => client.GetMeasurement("rated_power", (double)client.GetUnsignedInteger(30073) / 1000);
        public static double GetMaximumActivePower(this HuaweiSun2000Client client) => (double)client.GetUnsignedInteger(30075) / 1000;
        public static double GetMaximumApparentPower(this HuaweiSun2000Client client) => (double)client.GetUnsignedInteger(30077) / 1000;
        public static double GetMaximumReactivePowerToGrid(this HuaweiSun2000Client client) => (double)client.GetInteger(30079) / 1000;
        public static double GetMaximumReactivePowerFromGrid(this HuaweiSun2000Client client) => (double)client.GetInteger(30081) / 1000;
        public static ushort GetState1(this HuaweiSun2000Client client) => client.GetUnsignedShort(32000);

        // TODO state2 (13) till alarm3 (17)

        public static double GetPVVoltage(this HuaweiSun2000Client client, int PVString) => (double)client.GetShort(32014 + 2 * PVString) / 10;
        public static double GetPVCurrent(this HuaweiSun2000Client client, int PVString) => (double)client.GetShort(32015 + 2 * PVString) / 100;
        public static Measurement<double> GetInputPower(this HuaweiSun2000Client client) => client.GetMeasurement("input_power", (double)client.GetInteger(32064) / 1000);
        public static double GetLineVoltageAB(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32066) / 10;
        public static double GetLineVoltageBC(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32067) / 10;
        public static double GetLineVoltageCA(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32068) / 10;
        public static double GetPhaseAVoltage(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32069) / 10;
        public static double GetPhaseBVoltage(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32070) / 10;
        public static double GetPhaseCVoltage(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32071) / 10;
        public static double GetPhaseACurrent(this HuaweiSun2000Client client) => (double)client.GetInteger(32072) / 1000;
        public static double GetPhaseBCurrent(this HuaweiSun2000Client client) => (double)client.GetInteger(32074) / 1000;
        public static double GetPhaseCCurrent(this HuaweiSun2000Client client) => (double)client.GetInteger(32076) / 1000;
        public static double GetPeakActivePowerOfCurrentDay(this HuaweiSun2000Client client) => (double)client.GetInteger(32078) / 1000;
        public static Measurement<double> GetActivePower(this HuaweiSun2000Client client) => client.GetMeasurement("active_power", (double)client.GetInteger(32080) / 1000);
        public static Measurement<double> GetReactivePower(this HuaweiSun2000Client client) => client.GetMeasurement("reactive_power", (double)client.GetInteger(32082) / 1000);
        public static double GetPowerFactor(this HuaweiSun2000Client client) => (double)client.GetShort(32084) / 1000;
        public static double GetGridFrequency(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32085) / 100;
        public static double GetEfficiency(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32086) / 100;
        public static double GetInternalTemperature(this HuaweiSun2000Client client) => (double)client.GetShort(32087) / 10;
        public static double GetInsulationResistance(this HuaweiSun2000Client client) => (double)client.GetUnsignedShort(32088) / 1000;

        // TODO Device status (44) to (55)

        public static BatteryRunningStatus GetBatteryRunningStatus(this HuaweiSun2000Client client) => (BatteryRunningStatus)client.GetUnsignedShort(37000);
        public static int GetBatteryChargeAndDischargePower(this HuaweiSun2000Client client) => client.GetInteger(37001);
        public static double GetBatteryCurrentDayChargeCapacity(this HuaweiSun2000Client client) => (double)client.GetUnsignedInteger(37015) / 100;
        public static double GetBatteryCurrentDayDischargeCapacity(this HuaweiSun2000Client client) => (double)client.GetUnsignedInteger(37017) / 100;
        public static Measurement<int> GetPowerMeterActivePower(this HuaweiSun2000Client client) => client.GetMeasurement("powermeter_active_power", client.GetInteger(37113));
        public static ushort GetNumberOfOptimizers(this HuaweiSun2000Client client) => client.GetUnsignedShort(37200);
        public static ushort GetNumberOfOnlineOptimizers(this HuaweiSun2000Client client) => client.GetUnsignedShort(37201);

        // TODO optimizer feature data (63) to end
    }

    public static class UnsignedShortExtensions
    {
        public static bool IsFlagSet(this ushort value, State1Flags flag)
        {
            return (value & (ushort)flag) == (ushort)flag;
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
}
