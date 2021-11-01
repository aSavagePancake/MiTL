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

            List<string> qlConfigTitles = new List<string>
            {
                "Quicklaunch1Name",
                "Quicklaunch1ExeName",
                "Quicklaunch1Path",
                "Quicklaunch2Name",
                "Quicklaunch2ExeName",
                "Quicklaunch2Path",
                "Quicklaunch3Name",
                "Quicklaunch3ExeName",
                "Quicklaunch3Path",
                "Quicklaunch4Name",
                "Quicklaunch4ExeName",
                "Quicklaunch4Path",
                "Quicklaunch5Name",
                "Quicklaunch5ExeName",
                "Quicklaunch5Path",
                "Quicklaunch6Name",
                "Quicklaunch6ExeName",
                "Quicklaunch6Path",
                "Quicklaunch7Name",
                "Quicklaunch7ExeName",
                "Quicklaunch7Path",
                "Quicklaunch8Name",
                "Quicklaunch8ExeName",
                "Quicklaunch8Path",
                "Quicklaunch9Name",
                "Quicklaunch9ExeName",
                "Quicklaunch9Path",
                "Quicklaunch10Name",
                "Quicklaunch10ExeName",
                "Quicklaunch10Path",
                "Quicklaunch11Name",
                "Quicklaunch11ExeName",
                "Quicklaunch11Path",
                "Quicklaunch12Name",
                "Quicklaunch12ExeName",
                "Quicklaunch12Path",
                "Quicklaunch13Name",
                "Quicklaunch13ExeName",
                "Quicklaunch13Path",
                "Quicklaunch14Name",
                "Quicklaunch14ExeName",
                "Quicklaunch14Path",
                "Quicklaunch15Name",
                "Quicklaunch15ExeName",
                "Quicklaunch15Path",
                "Quicklaunch16Name",
                "Quicklaunch16ExeName",
                "Quicklaunch16Path",
            };

            foreach (string qlConfigTitle in qlConfigTitles)
            {
                IniWrite(qlConfigTitle, "");
            }
        }
    }
}