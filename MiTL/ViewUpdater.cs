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

        public static void UpdateHardwareData(string memLoad, string coreClock, string coreLoad, string coreTemp)
        {
            MainWindow.TbMemLoad.Content = memLoad;
            MainWindow.TbCpuClock.Content = coreClock;
            MainWindow.TbCpuLoad.Content = coreLoad;
            MainWindow.TbCpuTemp.Content = coreTemp;
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