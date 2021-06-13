using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;

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

        //xml file management ... >
        public static string XmlRead(string fullXmlFilePath)
        {
            string str = string.Empty;

            XDocument doc = XDocument.Load(fullXmlFilePath);
            System.Collections.Generic.IEnumerable<XElement> selectors = from elements in doc.Elements("profile").Elements("info")
                                                                         select elements;

            foreach (XElement element in selectors)
            {
                str = element.Element("profile_name")?.Value;
            }
            return str;
        }

        public void WriteConfigDefaults()
        {
            string powerPlanBalanced = ListManager.PowerPlanList.ElementAt(0);
            string powerPlanPerformance = ListManager.PowerPlanList.ElementAt(1);
            string defaultMonitoringEnabled = Properties.Resources.DefaultMonitoringEnabled;
            string defaultAppTheme = Properties.Resources.DefaultAppTheme;
            string defaultGameModeHotKey = Properties.Resources.DefaultGameModeHotKey;
            string defaultAudioDeviceSwitchHotKey = Properties.Resources.DefaultAudioDeviceSwitchHotKey;
            string defaultExitAppHotKey = Properties.Resources.DefaultExitAppHotkey;
            string audioDevice1 = ListManager.AudioDevicesList.ElementAt(0);
            string audioDevice2 = ListManager.AudioDevicesList.ElementAt(1);
            string defaultAudioDevice = ListManager.DefaultAudioDevice;
            IniWrite("PowerPlanBalanced", powerPlanBalanced);
            IniWrite("PowerPlanPerformance", powerPlanPerformance);
            IniWrite("IsMonitoringEnabled", defaultMonitoringEnabled);
            IniWrite("AppTheme", defaultAppTheme);
            IniWrite("GameModeHotKey", defaultGameModeHotKey);
            IniWrite("AudioDeviceSwitchHotKey", defaultAudioDeviceSwitchHotKey);
            IniWrite("ExitAppHotKey", defaultExitAppHotKey);
            IniWrite("AudioDeviceNo1", audioDevice1);
            IniWrite("AudioDeviceNo2", audioDevice2);
            IniWrite("DefaultAudioDevice", defaultAudioDevice);
        }
    }
}