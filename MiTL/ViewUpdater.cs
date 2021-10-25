using ControlzEx.Theming;
using System.Windows;

namespace MiTL
{
    internal class ViewUpdater
    {
        private static readonly MainWindow MainWindow = (MainWindow)Application.Current.MainWindow;

        public static void SetAppTheme(Window main, string appTheme)
        {
            if (appTheme != null)
            {
                ThemeManager.Current.ChangeTheme(main, appTheme);
            }
        }

        public static void UpdateHardwareData(string memLoad, string memAvailable, string cpuClock, string cpuLoad, string cpuTemp, string cpuPower
            , string gpuClock, string gpuMemClock, string gpuLoad, string gpuMemLoad, string gpuTemp, string gpuPower, string gpuIMC, string gpuVE, string netUpload, string netDownload
            , string _fan1Speed, string _fan2Speed, string _fan3Speed, string _fan4Speed)
        {
            MainWindow.MemUsageLabel.Content = memLoad;
            MainWindow.MemAvailableLabel.Content = memAvailable;
            MainWindow.CpuClockLabel.Content = cpuClock;
            MainWindow.CpuLoadLabel.Content = cpuLoad;
            MainWindow.CpuTempLabel.Content = cpuTemp;
            MainWindow.CpuPowerLabel.Content = cpuPower;
            MainWindow.GpuClockLabel.Content = gpuClock;
            MainWindow.GpuMemClockLabel.Content = gpuMemClock;
            MainWindow.GpuTempLabel.Content = gpuTemp;
            MainWindow.GpuLoadLabel.Content = gpuLoad;
            MainWindow.GpuMemUsageLabel.Content = gpuMemLoad;
            MainWindow.GpuPowerLabel.Content = gpuPower;
            MainWindow.GpuIMCLabel.Content = gpuIMC;
            MainWindow.GpuVideoEngineLabel.Content = gpuVE;
            MainWindow.NetworkUploadLabel.Content = netUpload;
            MainWindow.NetworkDownloadLabel.Content = netDownload;
            MainWindow.fan1SpeedLabel.Content = _fan1Speed;
            MainWindow.fan2SpeedLabel.Content = _fan2Speed;
            MainWindow.fan3SpeedLabel.Content = _fan3Speed;
            MainWindow.fan4SpeedLabel.Content = _fan4Speed;
        }

        public static void UpdateTimerData(string timerResolution)
        {
            if (MainWindow.TimerResolutionLabel.ToString() != timerResolution)
            {
                MainWindow.TimerResolutionLabel.Content = timerResolution;
            }
        }
    }
}