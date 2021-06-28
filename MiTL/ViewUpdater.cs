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

        public static void UpdateHardwareData(string memLoad, string coreClock, string coreLoad, string coreTemp, string gpuClock, string gpuMemClock, string gpuLoad, string gpuMemLoad, string gpuTemp, string gpuFan, string gpuIMC, string gpuVE)
        {
            MainWindow.MemLoadLabel.Content = memLoad;
            MainWindow.CpuClockLabel.Content = coreClock;
            MainWindow.CpuLoadLabel.Content = coreLoad;
            MainWindow.CpuTempLabel.Content = coreTemp;
            MainWindow.GpuClockLabel.Content = gpuClock;
            MainWindow.GpuMemClockLabel.Content = gpuMemClock;
            MainWindow.GpuTempLabel.Content = gpuTemp;
            MainWindow.GpuLoadLabel.Content = gpuLoad;
            MainWindow.GpuMemLoadLabel.Content = gpuMemLoad;
            MainWindow.GpuFanSpeedLabel.Content = gpuFan;
            MainWindow.GpuIMCLabel.Content = gpuIMC;
            MainWindow.GpuVideoEngineLabel.Content = gpuVE;
        }

        public static void UpdateTimerData(string timerResolution)
        {
            if (MainWindow.TimerResolutionTileBadge.Badge.ToString() != timerResolution)
            {
                MainWindow.TimerResolutionTileBadge.Badge = timerResolution;
            }
        }
    }
}