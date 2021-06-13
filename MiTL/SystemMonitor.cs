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
        private const int Delay = 1000;

        public static void StartHardwareMonitor()
        {
            int roundValue;

            ThisPc = new Computer
            {
                IsMemoryEnabled = true,
                IsCpuEnabled = true
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
                    }
                    UpdateHardwareData(_memLoad, _cpuClock, _cpuLoad, _cpuTemp);
                }
            });
        }

        private static void UpdateHardwareData(string memLoad, string coreClock, string coreLoad, string coreTemp)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ViewUpdater.UpdateHardwareData(memLoad, coreClock, coreLoad, coreTemp);
            });
        }

        public static void StartTimerMonitor()
        {
            Task.Factory.StartNew(function: async () =>
            {
                while (true)
                {
                    string timerResolution = "Current Timer: " + Math.Round(ServiceManager.CurrentTimerRes() / 10000.0, 3) + "ms";
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