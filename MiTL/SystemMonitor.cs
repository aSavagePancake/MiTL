using LibreHardwareMonitor.Hardware;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace MiTL
{
    internal class SystemMonitor
    {
        public static Computer ThisPc;
        private static string _cpuClock;
        private static string _cpuLoad;
        private static string _cpuTemp;
        private static string _cpuPower;
        private static string _memLoad;
        private static string _memAvailable;
        private static string _gpuClock;
        private static string _gpuMemClock;
        private static string _gpuLoad;
        private static string _gpuBusLoad;
        private static string _gpuTemp;
        private static string _gpuPower;
        private static string _gpuIMC;
        private static string _gpuVE;
        private static string _netUpload;
        private static string _netDownload;
        private static string _fan1Speed;
        private static string _fan2Speed;
        private static string _fan3Speed;
        private static string _fan4Speed;
        private const int Delay = 500;

        public static void StartHardwareMonitor()
        {
            int roundValue;

            ThisPc = new Computer
            {
                IsMotherboardEnabled = true,
                IsMemoryEnabled = true,
                IsCpuEnabled = true,
                IsGpuEnabled = true,
                IsNetworkEnabled = true
            };
            ThisPc.Open();

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    foreach (IHardware hardware in ThisPc.Hardware)
                    {
                        hardware.Update();

                        if (hardware.Sensors.Length > 0)
                        {
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                // Memory data
                                if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("Memory", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _memLoad = roundValue + " %";
                                    }
                                    else
                                    {
                                        _memLoad = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Data && sensor.Name.Equals("Memory Available", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        string value = string.Format("{0:.##}", sensor.Value.GetValueOrDefault());
                                        _memAvailable = value + " GB";
                                    }
                                    else
                                    {
                                        _memAvailable = " -no data- ";
                                    }
                                }

                                // CPU data
                                if (sensor.SensorType == SensorType.Clock && sensor.Name.Equals("CPU Core #1", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _cpuClock = roundValue + " Mhz";
                                    }
                                    else
                                    {
                                        _cpuClock = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Temperature && sensor.Name.Equals("Core Max", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _cpuTemp = roundValue + " °C";
                                    }
                                    else
                                    {
                                        _cpuTemp = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("CPU Total", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _cpuLoad = roundValue + " %";
                                    }
                                    else
                                    {
                                        _cpuLoad = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Power && sensor.Name.Equals("CPU Package", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _cpuPower = roundValue + " W";
                                    }
                                    else
                                    {
                                        _cpuPower = " -no data- ";
                                    }
                                }

                                // GPU data
                                if (sensor.SensorType == SensorType.Clock && sensor.Name.Equals("GPU Core", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuClock = roundValue + " Mhz";
                                    }
                                    else
                                    {
                                        _gpuClock = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Clock && sensor.Name.Equals("GPU Memory", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuMemClock = roundValue + " Mhz";
                                    }
                                    else
                                    {
                                        _gpuMemClock = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Temperature && sensor.Name.Equals("GPU Core", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuTemp = roundValue + " °C";
                                    }
                                    else
                                    {
                                        _gpuTemp = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("GPU Core", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuLoad = roundValue + " %";
                                    }
                                    else
                                    {
                                        _gpuLoad = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("GPU Bus", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuBusLoad = roundValue + " %";
                                    }
                                    else
                                    {
                                        _gpuBusLoad = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("GPU Memory Controller", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuIMC = roundValue + " %";
                                    }
                                    else
                                    {
                                        _gpuIMC = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("GPU Video Engine", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuVE = roundValue + " %";
                                    }
                                    else
                                    {
                                        _gpuVE = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Power && sensor.Name.Equals("GPU Package", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        _gpuPower = roundValue + " W";
                                    }
                                    else
                                    {
                                        _gpuPower = " -no data- ";
                                    }
                                }

                                // Network data
                                if (sensor.SensorType == SensorType.Throughput && sensor.Name.Equals("Upload Speed", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        if (roundValue >= 1000000000.0)
                                        {
                                            _netUpload = string.Format("{0:##.##}", roundValue / 1000000000.0) + " GB/s";
                                        }
                                        else if (roundValue >= 1000000.0)
                                        {
                                            _netUpload = string.Format("{0:##.##}", roundValue / 1000000.0) + " MB/s";
                                        }
                                        else if (roundValue >= 1000.0)
                                        {
                                            _netUpload = string.Format("{0:##.##}", roundValue / 1000) + " KB/s";
                                        }
                                        else if (roundValue > 0 && roundValue < 1000.0)
                                        {
                                            _netUpload = "0 KB/s";
                                        }
                                    }
                                    else
                                    {
                                        _netUpload = " -no data- ";
                                    }
                                }
                                if (sensor.SensorType == SensorType.Throughput && sensor.Name.Equals("Download Speed", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (sensor.Value != null)
                                    {
                                        roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                        if (roundValue >= 1000000000.0)
                                        {
                                            _netDownload = string.Format("{0:##.##}", roundValue / 1000000000.0) + " GB/s";
                                        }
                                        else if (roundValue >= 1000000.0)
                                        {
                                            _netDownload = string.Format("{0:##.##}", roundValue / 1000000.0) + " MB/s";
                                        }
                                        else if (roundValue >= 1000.0)
                                        {
                                            _netDownload = string.Format("{0:##.##}", roundValue / 1000) + " KB/s";
                                        }
                                        else if (roundValue > 0 && roundValue < 1000.0)
                                        {
                                            _netDownload = "0 KB/s";
                                        }
                                    }
                                    else
                                    {
                                        _netDownload = " -no data- ";
                                    }
                                }
                            }
                        }

                        // ALL Fans: speed data
                        foreach (IHardware subhardware in hardware.SubHardware)
                        {
                            subhardware.Update();

                            if (subhardware.Sensors.Length > 0)
                            {
                                foreach (ISensor sensor in subhardware.Sensors)
                                {
                                    if (sensor.SensorType == SensorType.Fan && sensor.Name.Equals("Fan #1", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (sensor.Value != null)
                                        {
                                            roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                            _fan1Speed = roundValue + " RPM";
                                        }
                                        else
                                        {
                                            _fan1Speed = " -no data- ";
                                        }
                                    }
                                    if (sensor.SensorType == SensorType.Fan && sensor.Name.Equals("Fan #2", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (sensor.Value != null)
                                        {
                                            roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                            _fan2Speed = roundValue + " RPM";
                                        }
                                        else
                                        {
                                            _fan2Speed = " -no data- ";
                                        }
                                    }
                                    if (sensor.SensorType == SensorType.Fan && sensor.Name.Equals("Fan #3", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (sensor.Value != null)
                                        {
                                            roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                            _fan3Speed = roundValue + " RPM";
                                        }
                                        else
                                        {
                                            _fan3Speed = " -no data- ";
                                        }
                                    }
                                    if (sensor.SensorType == SensorType.Fan && sensor.Name.Equals("Fan #4", StringComparison.OrdinalIgnoreCase))
                                    {
                                        if (sensor.Value != null)
                                        {
                                            roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                            _fan4Speed = roundValue + " RPM";
                                        }
                                        else
                                        {
                                            _fan4Speed = " -no data- ";
                                        }
                                    }
                                }
                            }
                        }
                    }
                    await Task.Delay(Delay).ConfigureAwait(false);
                    UpdateHardwareData();
                    await Task.Delay(Delay).ConfigureAwait(false);
                }
            });
        }

        private static void UpdateHardwareData()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ViewUpdater.UpdateHardwareData(_memLoad, _memAvailable, _cpuClock, _cpuLoad, _cpuTemp, _cpuPower
                        , _gpuClock, _gpuMemClock, _gpuLoad, _gpuBusLoad, _gpuTemp, _gpuPower, _gpuIMC, _gpuVE, _netUpload, _netDownload
                        , _fan1Speed, _fan2Speed, _fan3Speed, _fan4Speed);
            });
        }
    }
}