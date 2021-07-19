using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;

namespace MiTL
{
    internal class ListManager
    {
        private static readonly MainWindow MainWindow = (MainWindow)Application.Current.MainWindow;
        private static readonly MMDeviceEnumerator Enumerator = new MMDeviceEnumerator();

        internal static List<string> AudioDevicesList
        {
            get
            {
                List<string> audioDevicesList = new List<string>();
                foreach (MMDevice endpoint in
                    Enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
                {
                    string audioDevices = endpoint.FriendlyName;
                    string input = audioDevices;
                    input = input.Substring(0, input.IndexOf("(", StringComparison.Ordinal));
                    audioDevicesList.Add(input);
                }
                return audioDevicesList;
            }
        }

        internal static string DefaultAudioDevice
        {
            get
            {
                string defaultAudioOutput = Enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia).FriendlyName;
                string input = defaultAudioOutput;
                input = input.Substring(0, input.IndexOf("(", StringComparison.Ordinal));
                return input;
            }
        }

        [DllImport("PowrProf.dll")]
        public static extern uint PowerEnumerate(IntPtr rootPowerKey, IntPtr schemeGuid,
            IntPtr subGroupOfPowerSettingGuid, uint accessFlags, uint index, ref Guid buffer, ref uint bufferSize);

        [DllImport("PowrProf.dll")]
        public static extern uint PowerReadFriendlyName(IntPtr rootPowerKey, ref Guid schemeGuid,
            IntPtr subGroupOfPowerSettingGuid, IntPtr powerSettingGuid, IntPtr buffer, ref uint bufferSize);

        public enum AccessFlags : uint
        {
            AccessScheme = 16,
        }

        private static string ReadFriendlyName(Guid schemeGuid)
        {
            uint sizeName = 1024;
            IntPtr pSizeName = Marshal.AllocHGlobal((int)sizeName);
            string friendlyName;

            try
            {
                PowerReadFriendlyName(IntPtr.Zero, ref schemeGuid, IntPtr.Zero, IntPtr.Zero, pSizeName, ref sizeName);
                friendlyName = Marshal.PtrToStringUni(pSizeName);
            }
            finally
            {
                Marshal.FreeHGlobal(pSizeName);
            }

            return friendlyName;
        }

        public static IEnumerable<Guid> GetAll()
        {
            Guid schemeGuid = Guid.Empty;
            uint sizeSchemeGuid = (uint)Marshal.SizeOf(typeof(Guid));
            uint schemeIndex = 0;

            while (PowerEnumerate(IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, (uint)AccessFlags.AccessScheme, schemeIndex, ref schemeGuid, ref sizeSchemeGuid) == 0)
            {
                yield return schemeGuid;
                schemeIndex++;
            }
        }

        internal static List<string> PowerPlanList
        {
            get
            {
                List<string> powerPlanList = new List<string>();
                IEnumerable<Guid> guidPlans = GetAll();

                foreach (Guid guidPlan in guidPlans)
                {
                    string compareString = ReadFriendlyName(guidPlan).ToLower();

                    if (compareString.Contains("balanced"))
                    {
                        powerPlanList.Insert(0, guidPlan.ToString());
                    }
                    if (compareString.Contains("performance"))
                    {
                        powerPlanList.Insert(1, guidPlan.ToString());
                    }
                }
                return powerPlanList;
            }
        }

        internal IEnumerable<Grid> NavigationGrids
        {
            get
            {
                List<Grid> navGrids = new List<Grid>
                {
                    MainWindow.GridPerformance,
                    MainWindow.GridMonitoring,
                    MainWindow.GridQuickLaunch,
                    MainWindow.GridSettings,
                    MainWindow.GridAbout
                };
                return navGrids;
            }
        }

        internal IEnumerable<Border> NavigationIndicators
        {
            get
            {
                List<Border> navIndicators = new List<Border>
                {
                    MainWindow.NavMonitoringIndicator,
                    MainWindow.NavPerformanceIndicator,
                    MainWindow.NavQuickLaunchIndicator,
                    MainWindow.NavSettingsIndicator,
                    MainWindow.NavAboutIndicator
                };
                return navIndicators;
            }
        }

        internal IEnumerable<Border> SettingsTabIndicators
        {
            get
            {
                List<Border> tabIndicators = new List<Border>
                {
                    MainWindow.SettingsTimerServiceindicator,
                    MainWindow.SettingsAudioDevicesindicator,
                    MainWindow.SettingsQuickLaunchindicator,
                    MainWindow.SettingsHotkeysindicator,
                    MainWindow.SettingsThemeindicator
                };
                return tabIndicators;
            }
        }

        internal static List<string> MsiAbProfileList
        {
            get
            {
                var hotKeyMsiAb = new List<string>
                {
                    "1",
                    "2",
                    "3",
                    "4",
                    "5"
                };
                return hotKeyMsiAb;
            }
        }

        internal static List<string> HotKey
        {
            get
            {
                List<string> hotKey = new List<string>
                {
                    "--",
                    "A",
                    "B",
                    "C",
                    "D",
                    "E",
                    "F",
                    "G",
                    "H",
                    "I",
                    "J",
                    "K",
                    "L",
                    "M",
                    "N",
                    "O",
                    "P",
                    "Q",
                    "R",
                    "S",
                    "T",
                    "U",
                    "V",
                    "W",
                    "X",
                    "Y",
                    "Z",
                    "0",
                    "1",
                    "2",
                    "3",
                    "4",
                    "5",
                    "6",
                    "7",
                    "8",
                    "9",
                    "F1",
                    "F2",
                    "F3",
                    "F4",
                    "F5",
                    "F6",
                    "F7",
                    "F8",
                    "F9",
                    "F10",
                    "F11",
                    "F12"
                };
                return hotKey;
            }
        }
    }
}