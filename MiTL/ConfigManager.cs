using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MiTL
{
    internal class ConfigManager
    {
        private readonly string _path;
        private readonly string _settingsFile = Properties.Resources.SettingsFile;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal, int size, string filePath);

        //ini file management ... >
        public ConfigManager(string iniPath = null)
        {
            _path = new FileInfo(iniPath ?? _settingsFile).FullName;
        }

        public string IniRead(string key, string section = null)
        {
            StringBuilder retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? _settingsFile, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void IniWrite(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? _settingsFile, key, value, _path);
        }

        public void WriteConfigDefaults()
        {
            string defaultAppTheme = Properties.Resources.DefaultAppTheme;
            IniWrite("AppTheme", defaultAppTheme);

            if (MainWindow.PowerPlanList.Count == 1)
            {
                string powerPlanBalanced = MainWindow.PowerPlanList.ElementAt(0);
                IniWrite("PowerPlanBalanced", powerPlanBalanced);
            }

            if (MainWindow.PowerPlanList.Count == 2)
            {
                string powerPlanBalanced = MainWindow.PowerPlanList.ElementAt(0);
                IniWrite("PowerPlanBalanced", powerPlanBalanced);
                string powerPlanPerformance = MainWindow.PowerPlanList.ElementAt(1);
                IniWrite("PowerPlanPerformance", powerPlanPerformance);
            }

            string closeOnLaunch = Properties.Resources.CloseOnLaunch;
            IniWrite("CloseOnLaunch", closeOnLaunch);

            List<string> launcherConfigTitles = new List<string>
            {
                "Launcher1Name",
                "Launcher1Path",
                "Launcher2Name",
                "Launcher2Path",
                "Launcher3Name",
                "Launcher3Path",
                "Launcher4Name",
                "Launcher4Path",
                "Launcher5Name",
                "Launcher5Path",
                "Launcher6Name",
                "Launcher6Path",
                "Launcher7Name",
                "Launcher7Path",
                "Launcher8Name",
                "Launcher8Path",
                "Launcher9Name",
                "Launcher9Path",
                "Launcher10Name",
                "Launcher10Path",
                "Launcher11Name",
                "Launcher11Path",
                "Launcher12Name",
                "Launcher12Path",
                "Launcher13Name",
                "Launcher13Path",
                "Launcher14Name",
                "Launcher14Path",
                "Launcher15Name",
                "Launcher15Path",
                "Launcher16Name",
                "Launcher16Path",
            };

            foreach (string launcherConfigTitle in launcherConfigTitles)
            {
                IniWrite(launcherConfigTitle, "");
            }
        }
    }
}