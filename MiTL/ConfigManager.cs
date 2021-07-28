using System.Collections.Generic;
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
            string defaultAppTheme = Properties.Resources.DefaultAppTheme;
            string defaultGameModeHotKey = Properties.Resources.DefaultGameModeHotKey;
            string defaultAudioDeviceSwitchHotKey = Properties.Resources.DefaultAudioDeviceSwitchHotKey;
            string defaultExitAppHotKey = Properties.Resources.DefaultExitAppHotkey;
            string msiabFilePath = Properties.Resources.MSIAB_FilePath;
            string defaultStockProfile = Properties.Resources.DefaultGpuStockProfile;
            string defaultOcProfile = Properties.Resources.DefaultGpuOCProfile;
            string closeAfterburner = Properties.Resources.CloseAfterburner;
            IniWrite("StockProfile", defaultStockProfile);
            IniWrite("OCProfile", defaultOcProfile);
            IniWrite("CloseAfterburner", closeAfterburner);
            IniWrite("AppTheme", defaultAppTheme);
            IniWrite("GameModeHotKey", defaultGameModeHotKey);
            IniWrite("AudioDeviceSwitchHotKey", defaultAudioDeviceSwitchHotKey);
            IniWrite("ExitAppHotKey", defaultExitAppHotKey);

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

            if (ListManager.AudioDevicesList.Count == 1)
            {
                string audioDevice = ListManager.AudioDevicesList.ElementAt(0);
                IniWrite("AudioDevice1", audioDevice);
                IniWrite("AudioDevice2", "-");
            }
            else if (ListManager.AudioDevicesList.Count > 1)
            {
                foreach ((string value, int i) item in ListManager.AudioDevicesList.Select((value, i) => (value, i)))
                {
                    string value = item.value;
                    int index = item.i + 1;
                    string deviceNumber = "AudioDevice" + index.ToString();
                    string audioDevice = ListManager.AudioDevicesList.ElementAt(index);
                    IniWrite(deviceNumber, audioDevice);
                }
            }

            string defaultAudioDevice = ListManager.DefaultAudioDevice;
            IniWrite("DefaultAudioDevice", defaultAudioDevice);

            List<string> qlConfig = new List<string>
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

            foreach (string qlConfigTitle in qlConfig)
            {
                IniWrite(qlConfigTitle, "");
            }
        }
    }
}