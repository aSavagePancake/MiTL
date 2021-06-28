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
        private static string _memLoad;
        private static string _gpuClock;
        private static string _gpuMemClock;
        private static string _gpuLoad;
        private static string _gpuMemLoad;
        private static string _gpuTemp;
        private static string _gpuFan;
        private static string _gpuIMC;
        private static string _gpuVE;

        private const int Delay = 1000;

        public static void StartHardwareMonitor()
        {
            int roundValue;

            ThisPc = new Computer
            {
                IsMemoryEnabled = true,
                IsCpuEnabled = true,
                IsGpuEnabled = true
            };

            Task.Factory.StartNew(async () =>
            {
                ThisPc.Open();

                while (true)
                {
                    await Task.Delay(Delay).ConfigureAwait(false);

                    foreach (IHardware hardware in ThisPc.Hardware)
                    {
                        hardware.Update();

                        if (hardware.HardwareType == HardwareType.Memory)
                        {
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                switch (sensor.SensorType)
                                {
                                    case SensorType.Load:
                                        {
                                            if (sensor.Name.Equals("Memory"))
                                            {
                                                if (sensor.Value != null && sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _memLoad = roundValue + " %";
                                                }
                                                else
                                                {
                                                    _memLoad = " -no data- ";
                                                }
                                            }
                                            break;
                                        }
                                    case SensorType.Voltage:
                                        break;

                                    case SensorType.Clock:
                                        break;

                                    case SensorType.Fan:
                                        break;

                                    case SensorType.Flow:
                                        break;

                                    case SensorType.Control:
                                        break;

                                    case SensorType.Level:
                                        break;

                                    case SensorType.Factor:
                                        break;

                                    case SensorType.Power:
                                        break;

                                    case SensorType.Data:
                                        break;

                                    case SensorType.SmallData:
                                        break;

                                    case SensorType.Throughput:
                                        break;

                                    case SensorType.Temperature:
                                        break;

                                    case SensorType.Frequency:
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }

                        if (hardware.HardwareType == HardwareType.Cpu)
                        {
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                switch (sensor.SensorType)
                                {
                                    case SensorType.Clock:
                                        {
                                            if (sensor.Name.Equals("CPU Core #1"))
                                            {
                                                if (sensor.Value != null && sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _cpuClock = roundValue + " Mhz";
                                                }
                                                else
                                                {
                                                    _cpuClock = " -no data- ";
                                                }
                                            }
                                            break;
                                        }
                                    case SensorType.Temperature:
                                        {
                                            if (sensor.Name.Equals("CPU Core #1"))
                                            {
                                                if (sensor.Value != null && sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _cpuTemp = roundValue + " °C";
                                                }
                                                else
                                                {
                                                    _cpuTemp = " -no data- ";
                                                }
                                            }
                                            break;
                                        }
                                    case SensorType.Load:
                                        if (sensor.Name.Equals("CPU Total"))
                                        {
                                            if (sensor.Value != null && sensor.Value.Value >= 0)
                                            {
                                                roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                _cpuLoad = roundValue + " %";
                                            }
                                            else
                                            {
                                                _cpuLoad = " -no data- ";
                                            }
                                        }
                                        break;

                                    case SensorType.Voltage:
                                        break;

                                    case SensorType.Frequency:
                                        break;

                                    case SensorType.Fan:
                                        break;

                                    case SensorType.Flow:
                                        break;

                                    case SensorType.Control:
                                        break;

                                    case SensorType.Level:
                                        break;

                                    case SensorType.Factor:
                                        break;

                                    case SensorType.Power:
                                        break;

                                    case SensorType.Data:
                                        break;

                                    case SensorType.SmallData:
                                        break;

                                    case SensorType.Throughput:
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }

                        if (hardware.HardwareType == HardwareType.GpuNvidia)
                        {
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                switch (sensor.SensorType)
                                {
                                    case SensorType.Clock:
                                        {
                                            if (sensor.Name.Equals("GPU Core"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuClock = roundValue + " Mhz";
                                                }
                                                else
                                                {
                                                    _gpuClock = " -no data- ";
                                                }
                                            }
                                            if (sensor.Name.Equals("GPU Memory"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuMemClock = roundValue + " Mhz";
                                                }
                                                else
                                                {
                                                    _gpuMemClock = " -no data- ";
                                                }
                                            }
                                            break;
                                        }
                                    case SensorType.Temperature:
                                        {
                                            if (sensor.Name.Equals("GPU Core"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuTemp = roundValue + " °C";
                                                }
                                                else
                                                {
                                                    _gpuTemp = " -no data- ";
                                                }
                                            }
                                            break;
                                        }
                                    case SensorType.Load:
                                        {
                                            if (sensor.Name.Equals("GPU Core"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuLoad = roundValue + " %";
                                                }
                                                else
                                                {
                                                    _gpuLoad = " -no data- ";
                                                }
                                            }
                                            if (sensor.Name.Equals("GPU Memory"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuMemLoad = roundValue + " %";
                                                }
                                                else
                                                {
                                                    _gpuMemLoad = " -no data- ";
                                                }
                                            }
                                            if (sensor.Name.Equals("GPU Memory Controller"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuIMC = roundValue + " %";
                                                }
                                                else
                                                {
                                                    _gpuIMC = " -no data- ";
                                                }
                                            }
                                            if (sensor.Name.Equals("GPU Video Engine"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuVE = roundValue + " %";
                                                }
                                                else
                                                {
                                                    _gpuVE = " -no data- ";
                                                }
                                            }
                                            break;
                                        }
                                    case SensorType.Control:
                                        {
                                            if (sensor.Name.Equals("GPU Fan"))
                                            {
                                                if (sensor.Value.Value >= 0)
                                                {
                                                    roundValue = (int)Math.Round(sensor.Value.GetValueOrDefault());
                                                    _gpuFan = roundValue + " %";
                                                }
                                                else
                                                {
                                                    _gpuFan = " -no data- ";
                                                }
                                            }
                                            break;
                                        }

                                    case SensorType.Voltage:
                                        break;

                                    case SensorType.Frequency:
                                        break;

                                    case SensorType.Flow:
                                        break;

                                    case SensorType.Fan:
                                        break;

                                    case SensorType.Level:
                                        break;

                                    case SensorType.Factor:
                                        break;

                                    case SensorType.Power:
                                        break;

                                    case SensorType.Data:
                                        break;

                                    case SensorType.SmallData:
                                        break;

                                    case SensorType.Throughput:
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                    }

                    UpdateHardwareData(_memLoad, _cpuClock, _cpuLoad, _cpuTemp, _gpuClock, _gpuMemClock, _gpuLoad, _gpuMemLoad, _gpuTemp, _gpuFan, _gpuIMC, _gpuVE);
                }
            });
        }

        private static void UpdateHardwareData(string memLoad, string cpuClock, string cpuLoad, string cpuTemp
            , string gpuClock, string gpuMemClock, string gpuLoad, string gpuMemLoad, string gpuTemp, string gpuFan, string gpuIMC, string gpuVE)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ViewUpdater.UpdateHardwareData(memLoad, cpuClock, cpuLoad, cpuTemp, gpuClock, gpuMemClock, gpuLoad, gpuMemLoad, gpuTemp, gpuFan, gpuIMC, gpuVE);
            });
        }

        public static void StartTimerMonitor()
        {
            Task.Factory.StartNew(function: async () =>
            {
                while (true)
                {
                    string timerResolution = "Current Timer: " + Math.Round(ServiceManager.CurrentTimerRes() / 10000.0, 2) + "ms";
                    UpdateTimerData(timerResolution);
                    await Task.Delay(Delay).ConfigureAwait(false);
                }
            });
        }

        private static void UpdateTimerData(string timerResolution)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ViewUpdater.UpdateTimerData(timerResolution);
            });
        }
    }
}