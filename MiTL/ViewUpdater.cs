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
            MainWindow.TbMemLoad.Content = memLoad;
            MainWindow.TbCpuClock.Content = coreClock;
            MainWindow.TbCpuLoad.Content = coreLoad;
            MainWindow.TbCpuTemp.Content = coreTemp;
            MainWindow.TbGpuClock.Content = gpuClock;
            MainWindow.TbGpuMemClock.Content = gpuMemClock;
            MainWindow.TbGpuTemp.Content = gpuTemp;
            MainWindow.TbGpuLoad.Content = gpuLoad;
            MainWindow.TbGpuMemLoad.Content = gpuMemLoad;
            MainWindow.TbGpuFanSpeed.Content = gpuFan;
            MainWindow.TbGpuIMC.Content = gpuIMC;
            MainWindow.TbGpuVideoEngine.Content = gpuVE;
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