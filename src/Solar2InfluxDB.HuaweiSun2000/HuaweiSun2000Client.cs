using FluentModbus;
using Microsoft.Extensions.Logging;
using Solar2InfluxDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solar2InfluxDB.HuaweiSun2000
{
    public class HuaweiSun2000Client : IMeasurementReader
    {
        private readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(20);
        private readonly TimeSpan ConnectCooldown = TimeSpan.FromSeconds(5);
        private readonly TimeSpan ModbusTimeout = TimeSpan.FromMilliseconds(3000);
        private readonly TimeSpan ConnectFirstReadDelay = TimeSpan.FromMilliseconds(1000);
        private readonly TimeSpan ConnectFirstReadExtendDelay = TimeSpan.FromMilliseconds(100);
        private readonly ModbusTcpClient modbusClient;
        private readonly ILogger<HuaweiSun2000Client> logger;
        private readonly Config config;

        public HuaweiSun2000Client(Config config, ILogger<HuaweiSun2000Client> logger)
        {
            modbusClient = new ModbusTcpClient
            {
                ConnectTimeout = (int)ModbusTimeout.TotalMilliseconds,
                ReadTimeout = (int)ModbusTimeout.TotalMilliseconds,
                WriteTimeout = (int)ModbusTimeout.TotalMilliseconds,
            };

            this.logger = logger;
            this.config = config;

            Hostname = config.Hostname ?? throw new Exception("No HuaweiSun2000 hostname specified");
        }

        public string Hostname { get; }
        public string Model { get; private set; }
        public string SerialNumber { get; private set; }

        async Task IMeasurementReader.Initialize()
        {
            try
            {
                IPAddress address = GetIPAddress();

                logger.LogInformation($"Connecting to {Hostname} at {address}");

                await Connect(address);

                Model = this.GetModel();
                SerialNumber = this.GetSerialNumber();

                logger.LogInformation($"Connected to {Model} with S/N {SerialNumber}");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Failed to initialize: {e.GetBaseException().Message}");
            }
        }

        async IAsyncEnumerable<MeasurementCollection> IMeasurementReader.ReadMeasurementsFromDevices()
        {
            if (config.Inverter?.ParametersToRead?.Any() ?? false)
            {
                yield return await this.GetInverterMeasurements(config.Inverter);
            }

            if (config.InverterState?.ParametersToRead?.Any() ?? false)
            {
                yield return await this.GetInverterStateMeasurements(config.InverterState);
            }

            if (config.PowerMeter?.ParametersToRead?.Any() ?? false)
            {
                yield return await this.GetPowerMeterMeasurements(config.PowerMeter);
            }

            if (config.PVStrings?.ParametersToRead?.Any() ?? false)
            {
                yield return await this.GetPVStringsMeasurements(config.PVStrings);
            }
        }

        internal string GetString(int address, int length)
        {
            var bytes = modbusClient.ReadHoldingRegisters<byte>(0, address, length);

            var stringBuilder = new StringBuilder();
            foreach (var b in bytes)
            {
                stringBuilder.Append((char)b);
            }

            return stringBuilder.ToString();
        }

        internal ushort GetUnsignedShort(int address) => modbusClient.ReadHoldingRegisters<ushort>(0, address, 1)[0];

        internal short GetShort(int address) => modbusClient.ReadHoldingRegisters<short>(0, address, 1)[0];

        internal uint GetUnsignedInteger(int address) => modbusClient.ReadHoldingRegisters<uint>(0, address, 2)[0];

        internal int GetInteger(int address) => modbusClient.ReadHoldingRegisters<int>(0, address, 2)[0];

        private async Task Connect(IPAddress address)
        {
            using (var connectTimeout = new CancellationTokenSource(ConnectTimeout))
            {
                TimeSpan delayTillFirstRead = ConnectFirstReadDelay;
                for (int attempt = 1; ; attempt++)
                {
                    if (connectTimeout.Token.IsCancellationRequested)
                    {
                        throw new TimeoutException("Failed to connect");
                    }

                    logger.LogDebug($"Connect attempt {attempt}...");

                    try
                    {
                        TimeSpan connectAndFirstReadTimeout = delayTillFirstRead + ModbusTimeout;

                        Task connectAndFirstRead = ConnectedAndFirstRead(address, delayTillFirstRead);

                        using (var timeoutCancellationTokenSource = new CancellationTokenSource(connectAndFirstReadTimeout))
                        {
                            var firstCompletedTask = await Task.WhenAny(
                                connectAndFirstRead,
                                Task.Delay(connectAndFirstReadTimeout, timeoutCancellationTokenSource.Token));

                            if (firstCompletedTask == connectAndFirstRead)
                            {
                                timeoutCancellationTokenSource.Cancel();

                                await connectAndFirstRead;

                                logger.LogDebug($"Connected with {(int)delayTillFirstRead.TotalMilliseconds}ms delay before first read");

                                return;
                            }
                            else
                            {
                                logger.LogDebug($"Connect failed with {(int)delayTillFirstRead.TotalMilliseconds}ms till first read. Retrying...");

                                delayTillFirstRead += ConnectFirstReadExtendDelay;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        logger.LogWarning(e, $"Connect attempt {attempt} failed: {e.GetBaseException().Message}");

                        await Task.Delay(ConnectCooldown);
                    }
                }
            }
        }

        private async Task ConnectedAndFirstRead(IPAddress address, TimeSpan delayTillFirstRead)
        {
            modbusClient.Connect(address, ModbusEndianness.BigEndian);

            if (!modbusClient.IsConnected)
            {
                throw new Exception("Failed to connect");
            }

            await Task.Delay(delayTillFirstRead);

            this.GetModelID();
        }

        private IPAddress GetIPAddress()
        {
            IPAddress[] addresses = Dns.GetHostAddresses(Hostname);

            if (!addresses.Any())
            {
                throw new Exception($"No IP address found for {Hostname}");
            }

            return addresses.First();
        }
    }
}
;