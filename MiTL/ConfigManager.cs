using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MiTL
{
    internal class ConfigManager
    {
        private readonly string _path;
        private readonly string _exe = Assembly.GetExecutingAssembly().GetName().Name;

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal, int size, string filePath);

        //ini file management ... >
        public ConfigManager(string iniPath = null)
        {
            _path = new FileInfo(iniPath ?? _exe + ".ini").FullName;
        }

        public string IniRead(string key, string section = null)
        {
            StringBuilder retVal = new StringBuilder(255);
            GetPrivateProfileString(section ?? _exe, key, "", retVal, 255, _path);
            return retVal.ToString();
        }

        public void IniWrite(string key, string value, string section = null)
        {
            WritePrivateProfileString(section ?? _exe, key, value, _path);
        }

        public void WriteConfigDefaults()
        {
            string defaultAppTheme = Properties.Resources.DefaultAppTheme;
            IniWrite("AppTheme", defaultAppTheme);

            if (ListManager.PowerPlanList.Count == 1)
            {
                string powerPlanBalanced = ListManager.PowerPlanList.ElementAt(0);
                IniWrite("PowerPlanBalanced", powerPlanBalanced);
            }

            if (ListManager.PowerPlanList.Count == 2)
            {
                string powerPlanBalanced = ListManager.PowerPlanList.ElementAt(0);
                IniWrite("PowerPlanBalanced", powerPlanBalanced);
                string powerPlanPerformance = ListManager.PowerPlanList.ElementAt(1);
                IniWrite("PowerPlanPerformance", powerPlanPerformance);
            }

            string closeOnQuicklaunch = Properties.Resources.CloseOnQuicklaunch;
            IniWrite("CloseOnQuicklaunch", closeOnQuicklaunch);

            List<string> qlConfigTitles = new List<string>
            {
                "Quicklaunch1Name",
                "Quicklaunch1Path",
                "Quicklaunch2Name",
                "Quicklaunch2Path",
                "Quicklaunch3Name",
                "Quicklaunch3Path",
                "Quicklaunch4Name",
                "Quicklaunch4Path",
                "Quicklaunch5Name",
                "Quicklaunch5Path",
                "Quicklaunch6Name",
                "Quicklaunch6Path",
                "Quicklaunch7Name",
                "Quicklaunch7Path",
                "Quicklaunch8Name",
                "Quicklaunch8Path",
                "Quicklaunch9Name",
                "Quicklaunch9Path",
                "Quicklaunch10Name",
                "Quicklaunch10Path",
                "Quicklaunch11Name",
                "Quicklaunch11Path",
                "Quicklaunch12Name",
                "Quicklaunch12Path",
                "Quicklaunch13Name",
                "Quicklaunch13Path",
                "Quicklaunch14Name",
                "Quicklaunch14Path",
                "Quicklaunch15Name",
                "Quicklaunch15Path",
                "Quicklaunch16Name",
                "Quicklaunch16Path",
            };

            foreach (string qlConfigTitle in qlConfigTitles)
            {
                IniWrite(qlConfigTitle, "");
            }
        }
    }
}