using MahApps.Metro.Controls;
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

        internal IEnumerable<Tile> ControlsMainTiles
        {
            get
            {
                List<Tile> controlsMainTiles = new List<Tile>
                {
                    MainWindow.PowerPlanTile,
                    MainWindow.TimerResolutionTile,
                    MainWindow.AudioDeviceSwitchTile
                };
                return controlsMainTiles;
            }
        }

        internal IEnumerable<Label> ControlsLabels
        {
            get
            {
                List<Label> ControlsLabels = new List<Label>
                {
                    MainWindow.TbCpuClock,
                    MainWindow.TbCpuLoad,
                    MainWindow.TbCpuTemp,
                    MainWindow.TbMemLoad
                };
                return ControlsLabels;
            }
        }

        internal IEnumerable<Frame> ControlsFrames
        {
            get
            {
                List<Frame> ControlsFrames = new List<Frame>
                {
                    MainWindow.CpuClockFrame,
                    MainWindow.CpuLoadFrame,
                    MainWindow.CpuTempFrame,
                    MainWindow.MemLoadFrame
                };
                return ControlsFrames;
            }
        }

        internal IEnumerable<Tile> SettingsTiles
        {
            get
            {
                List<Tile> settingsTiles = new List<Tile>
                {
                    MainWindow.SettingsThemeTile,
                    MainWindow.SettingsMonitoringTile,
                    MainWindow.SettingsTimerServiceTile,
                    MainWindow.SettingsAudioOutputsTile,
                    MainWindow.SettingsQuickLaunchTile,
                    MainWindow.SettingsHotKeysTile,
                };
                return settingsTiles;
            }
        }

        internal IEnumerable<Grid> SettingsGrids
        {
            get
            {
                List<Grid> settingsGrids = new List<Grid>
                {
                    MainWindow.SettingsThemeGrid,
                    MainWindow.SettingsMonitoringGrid,
                    MainWindow.SettingsTimerServiceGrid,
                    MainWindow.SettingsAudioOutputsGrid,
                    MainWindow.SettingsQuickLaunchGrid,
                    MainWindow.SettingsHotKeysGrid,
                };
                return settingsGrids;
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