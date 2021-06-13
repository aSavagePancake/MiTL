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
            MainWindow.TbMemLoad.Text = memLoad;
            MainWindow.TbCpuClock.Text = coreClock;
            MainWindow.TbCpuLoad.Text = coreLoad;
            MainWindow.TbCpuTemp.Text = coreTemp;
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