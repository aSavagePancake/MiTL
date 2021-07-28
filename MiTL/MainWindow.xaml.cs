using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiTL
{
    public partial class MainWindow
    {
        private static readonly ListManager ListManager = new ListManager();
        private static readonly ConfigManager ConfigManager = new ConfigManager();
        private static Grid _viewGridName = new Grid();
        private static Border _viewIndicatorName = new Border();
        private static string _gpuDefaultProfile;
        private static string _gpuOCProfile;
        private static string _closeAfterburner;
        private static string _activePowerPlan;
        private static string _powerPlanBalanced;
        private static string _powerPlanPerformance;
        private static string _appTheme;
        private static string _gameModeHotKey;
        private static string _audioDeviceSwitchHotKey;
        private static string _exitAppHotKey;
        private static string _audioDevice1;
        private static string _audioDevice2;
        private static string _defaultAudioDevice;
        private static string _quicklaunch1Name;
        private static string _quicklaunch1Path;
        private static string _quicklaunch2Name;
        private static string _quicklaunch2Path;
        private static string _quicklaunch3Name;
        private static string _quicklaunch3Path;
        private static string _quicklaunch4Name;
        private static string _quicklaunch4Path;
        private static string _quicklaunch5Name;
        private static string _quicklaunch5Path;
        private static string _quicklaunch6Name;
        private static string _quicklaunch6Path;
        private static string _quicklaunch7Name;
        private static string _quicklaunch7Path;
        private static string _quicklaunch8Name;
        private static string _quicklaunch8Path;
        private static string _quicklaunch9Name;
        private static string _quicklaunch9Path;
        private static string _quicklaunch10Name;
        private static string _quicklaunch10Path;
        private static string _quicklaunch11Name;
        private static string _quicklaunch11Path;
        private static string _quicklaunch12Name;
        private static string _quicklaunch12Path;
        private static string _quicklaunch13Name;
        private static string _quicklaunch13Path;
        private static string _quicklaunch14Name;
        private static string _quicklaunch14Path;
        private static string _quicklaunch15Name;
        private static string _quicklaunch15Path;
        private static string _quicklaunch16Name;
        private static string _quicklaunch16Path;
        private static string qlTileNumber = "";

        public MainWindow()
        {
            //ignore Theme ResourceDictionary before initialize as setting whichever theme (_appTheme) saved in .ini
            Application.Current.Resources.MergedDictionaries.RemoveAt(0);
            InitializeComponent();

            //add MainWindow event handlers
            Closed += MainWindow_Closed;
            Activated += MainWindow_Activated;

            //run startup checks, read all config settings and set corresponding UI elements
            Startup();
            ShowActivePowerPlan();
            ShowDefaultAudioDevice();
            SetupComboListSources();

            //start Monitoring
            StartTimerMonitor();
            StartHardwareMonitor();
        }

        //app startup checks
        private void Startup()
        {
            //check for duplicate process instances
            ProcessManager.TerminateDuplicate();

            //check if config file exists, if not write default config file
            string myConfigManager = Properties.Resources.MyConfigManager;
            if (!File.Exists(myConfigManager))
            {
                ConfigManager.WriteConfigDefaults();
            }
            ReadSettings();

            //set app theme
            ViewUpdater.SetAppTheme(this, _appTheme);
        }

        //read in values from ini file
        private static void ReadSettings()
        {
            _gpuDefaultProfile = ConfigManager.IniRead("StockProfile");
            _gpuOCProfile = ConfigManager.IniRead("OCProfile");
            _closeAfterburner = ConfigManager.IniRead("CloseAfterburner");
            _powerPlanBalanced = ConfigManager.IniRead("PowerPlanBalanced");
            _powerPlanPerformance = ConfigManager.IniRead("PowerPlanPerformance");
            _appTheme = ConfigManager.IniRead("AppTheme");
            _gameModeHotKey = ConfigManager.IniRead("GameModeHotKey");
            _audioDeviceSwitchHotKey = ConfigManager.IniRead("AudioDeviceSwitchHotKey");
            _exitAppHotKey = ConfigManager.IniRead("ExitAppHotKey");
            _audioDevice1 = ConfigManager.IniRead("AudioDevice1");
            _audioDevice2 = ConfigManager.IniRead("AudioDevice2");
            _defaultAudioDevice = ConfigManager.IniRead("DefaultAudioDevice");
            _quicklaunch1Name = ConfigManager.IniRead("Quicklaunch1Name");
            _quicklaunch1Path = ConfigManager.IniRead("Quicklaunch1Path");
            _quicklaunch2Name = ConfigManager.IniRead("Quicklaunch2Name");
            _quicklaunch2Path = ConfigManager.IniRead("Quicklaunch2Path");
            _quicklaunch3Name = ConfigManager.IniRead("Quicklaunch3Name");
            _quicklaunch3Path = ConfigManager.IniRead("Quicklaunch3Path");
            _quicklaunch4Name = ConfigManager.IniRead("Quicklaunch4Name");
            _quicklaunch4Path = ConfigManager.IniRead("Quicklaunch4Path");
            _quicklaunch5Name = ConfigManager.IniRead("Quicklaunch5Name");
            _quicklaunch5Path = ConfigManager.IniRead("Quicklaunch5Path");
            _quicklaunch6Name = ConfigManager.IniRead("Quicklaunch6Name");
            _quicklaunch6Path = ConfigManager.IniRead("Quicklaunch6Path");
            _quicklaunch7Name = ConfigManager.IniRead("Quicklaunch7Name");
            _quicklaunch7Path = ConfigManager.IniRead("Quicklaunch7Path");
            _quicklaunch8Name = ConfigManager.IniRead("Quicklaunch8Name");
            _quicklaunch8Path = ConfigManager.IniRead("Quicklaunch8Path");
            _quicklaunch9Name = ConfigManager.IniRead("Quicklaunch9Name");
            _quicklaunch9Path = ConfigManager.IniRead("Quicklaunch9Path");
            _quicklaunch10Name = ConfigManager.IniRead("Quicklaunch10Name");
            _quicklaunch10Path = ConfigManager.IniRead("Quicklaunch10Path");
            _quicklaunch11Name = ConfigManager.IniRead("Quicklaunch11Name");
            _quicklaunch11Path = ConfigManager.IniRead("Quicklaunch11Path");
            _quicklaunch12Name = ConfigManager.IniRead("Quicklaunch12Name");
            _quicklaunch12Path = ConfigManager.IniRead("Quicklaunch12Path");
            _quicklaunch13Name = ConfigManager.IniRead("Quicklaunch13Name");
            _quicklaunch13Path = ConfigManager.IniRead("Quicklaunch13Path");
            _quicklaunch14Name = ConfigManager.IniRead("Quicklaunch14Name");
            _quicklaunch14Path = ConfigManager.IniRead("Quicklaunch14Path");
            _quicklaunch15Name = ConfigManager.IniRead("Quicklaunch15Name");
            _quicklaunch15Path = ConfigManager.IniRead("Quicklaunch15Path");
            _quicklaunch16Name = ConfigManager.IniRead("Quicklaunch16Name");
            _quicklaunch16Path = ConfigManager.IniRead("Quicklaunch16Path");
        }

        //show which Power Plan is active
        private void ShowActivePowerPlan()
        {
            // get active power plan
            Process getActivePowerPlan = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "powercfg",
                    Arguments = "/GetActiveScheme",
                    RedirectStandardError = true,
                    RedirectStandardOutput = true
                }
            };
            getActivePowerPlan.Start();

            string activePlan = getActivePowerPlan.StandardOutput.ReadToEnd();
            getActivePowerPlan.WaitForExit();

            if (activePlan.Contains(_powerPlanBalanced))
            {
                _activePowerPlan = " Balanced ";
            }
            if (activePlan.Contains(_powerPlanPerformance))
            {
                _activePowerPlan = " High Performance ";
            }
            if (PowerPlanBadge.ToString() != _activePowerPlan)
            {
                PowerPlanBadge.Badge = _activePowerPlan;
            }
        }

        //show which Audio profile is active
        private void ShowDefaultAudioDevice()
        {
            string defaultAudioDevice = " " + _defaultAudioDevice + " ";
            string defaultAudioDeviceBadge = AudioDeviceBadge.Badge.ToString();

            if (defaultAudioDeviceBadge != defaultAudioDevice)
            {
                string formattedTitle = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(_defaultAudioDevice.ToLower());
                AudioDeviceBadge.Badge = " " + formattedTitle + " ";
            }
        }

        private void SetupComboListSources()
        {
            //define List sources
            GameModeHotKey.ItemsSource = ListManager.HotKey;
            AudioDevice1.ItemsSource = ListManager.AudioDevicesList;
            AudioDevice2.ItemsSource = ListManager.AudioDevicesList;
            AudioDeviceHotKey.ItemsSource = ListManager.HotKey;
            ExitAppHotKey.ItemsSource = ListManager.HotKey;

            //show relative list indexes
            SetComboListIndexes();
        }

        private void SetComboListIndexes()
        {
            int hotKeyGameMode = ListManager.HotKey.FindIndex(a => a.Contains(_gameModeHotKey));
            GameModeHotKey.SelectedIndex = hotKeyGameMode;
            int audioDeviceSwitcher1 = ListManager.AudioDevicesList.FindIndex(a => a.Contains(_audioDevice1));
            AudioDevice1.SelectedIndex = audioDeviceSwitcher1;
            int audioDeviceSwitcher2 = ListManager.AudioDevicesList.FindIndex(a => a.Contains(_audioDevice2));
            AudioDevice2.SelectedIndex = audioDeviceSwitcher2;
            int hotKeyAudioSwitcher = ListManager.HotKey.FindIndex(a => a.Contains(_audioDeviceSwitchHotKey));
            AudioDeviceHotKey.SelectedIndex = hotKeyAudioSwitcher;
            int hotKeyExitApp = ListManager.HotKey.FindIndex(a => a.Contains(_exitAppHotKey));
            ExitAppHotKey.SelectedIndex = hotKeyExitApp;
        }

        private void ShowQuicklaunchTiles()
        {
            ReadSettings();

            string iconFolder = Environment.CurrentDirectory + @"\ql_icons\";
            string iconExtension = ".ico";

            List<string> qlIconName = new List<string>
                {
                    "ql1",
                    "ql2",
                    "ql3",
                    "ql4",
                    "ql5",
                    "ql6",
                    "ql7",
                    "ql8",
                    "ql9",
                    "ql10",
                    "ql11",
                    "ql12",
                    "ql13",
                    "ql14",
                    "ql15",
                    "ql16"
                };

            foreach (string item in qlIconName)
            {
                string qlTileNumber = item;
                int index = qlIconName.IndexOf(qlTileNumber);

                string qlName = QlNames[index];
                Tile qlTile = ListManager.QuicklaunchTiles[index];
                Image qlImage = ListManager.QuicklaunchTileImages[index];

                string iconPath = iconFolder + qlTileNumber + iconExtension;

                if (qlName.Length > 1)
                {
                    qlImage.Source = BitmapFromFile(iconPath);
                    qlTile.Title = qlName;
                }
            }
        }

        private ImageSource BitmapFromFile(string iconPath)
        {
            BitmapImage bitmap = new BitmapImage();
            using (FileStream fs = new FileStream(iconPath, FileMode.Open))
            {
                bitmap.BeginInit();
                bitmap.StreamSource = fs;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
            }
            return bitmap;
        }

        private static void StartTimerMonitor()
        {
            SystemMonitor.StartTimerMonitor();
        }

        public void StartHardwareMonitor()
        {
            SystemMonitor.StartHardwareMonitor();
        }

        //set Power Plan Scheme
        private void PowerPlanTile_OnCLick(object sender, RoutedEventArgs e)
        {
            string badgeString = PowerPlanBadge.Badge.ToString();

            switch (badgeString)
            {
                case " Balanced ":
                    ApplyPowerPlan(_powerPlanPerformance);
                    break;

                case " High Performance ":
                    ApplyPowerPlan(_powerPlanBalanced);
                    break;

                default:
                    break;
            }
        }

        //Apply selected profile
        private void ApplyPowerPlan(string powerPlan)
        {
            // set active power plan from Powercfg
            Process setActivePowerPlan = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "powercfg",
                    Arguments = "/s " + powerPlan
                }
            };
            setActivePowerPlan.Start();
            setActivePowerPlan.WaitForExit();
            ShowActivePowerPlan();
        }

        //switch default audio device
        private void AudioDeviceSwitchTile_OnClick(object sender, RoutedEventArgs e)
        {
            string device = "";
            if (_defaultAudioDevice == _audioDevice1)
            {
                device = _audioDevice2;
                if (device != "-")
                {
                    ConfigManager.IniWrite("DefaultAudioDevice", _audioDevice2);
                }
                else
                {
                    ShowMetroMessage("2nd Audio Device not assigned.", "If 2 or more Audio Devices then select it in the Settings Panel");
                }
            }
            if (_defaultAudioDevice == _audioDevice2)
            {
                device = _audioDevice1;
                ConfigManager.IniWrite("DefaultAudioDevice", _audioDevice1);
            }

            SetAudioDevice(device);
        }

        private void SetAudioDevice(string device)
        {
            if (device != "-")
            {
                // set default audio output device
                Process setDefaultAudioDevice = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        FileName = @"bin\nircmdc.exe",
                        Arguments = " setdefaultsounddevice " + "\"" + device + "\""
                    }
                };
                setDefaultAudioDevice.Start();
                setDefaultAudioDevice.WaitForExit();
                ReadSettings();
                ShowDefaultAudioDevice();
            }
        }

        private void TimerResolutionTile_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceManager.MiTLService.Refresh();
            if (!ServiceManager.ServiceExists())
            {
                ShowMetroMessage("Service Not Found.", "To change this, Install the Timer Resolution Service via Settings Panel");
            }
            else
            {
                ServiceManager.StartStopService();
            }
        }

        private void GpuDefaultProfileTile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("SetDefault");
        }

        private void GpuOCProfileTile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("SetOC");
        }

        //Apply selected profile
        private void ApplyProfile(string profile)
        {
            string args = null;

            switch (profile)
            {
                case "SetDefault":
                    args = "/Profile" + _gpuDefaultProfile;
                    break;

                case "SetOC":
                    args = "/Profile" + _gpuOCProfile;
                    break;

                default:
                    break;
            }

            //start MSIAfterburner using appropriate /profile switch
            string _msiabFile = Properties.Resources.MSIAB_FilePath;
            Process.Start(_msiabFile, args);

            if (ToggleCloseAfterburner.IsChecked == true)
            {
                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(1500);
                    ProcessManager.TerminateApp("MSIAfterburner");
                    ProcessManager.TerminateApp("RTSS");
                    ProcessManager.TerminateApp("RTSSHooksLoader");
                    ProcessManager.TerminateApp("RTSSHooksLoader64");
                });
            }
        }

        private void ToggleCloseAfterburner_Checked(object sender, RoutedEventArgs e)
        {
            ConfigManager.IniWrite("CloseAfterburner", "true");
        }

        private void ToggleCloseAfterburner_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigManager.IniWrite("CloseAfterburner", "false");
        }

        private void QLTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Tile SenderTile)
            {
                qlTileNumber = SenderTile.Tag.ToString();
            }

            int qlindexNumber = int.Parse(qlTileNumber) - 1;
            string qlName = QlNames[qlindexNumber];
            string qlPath = QlPaths[qlindexNumber];

            if (qlName.Length > 1)
            {
                if (File.Exists(qlPath))
                {
                    //ProcessManager.CloseAfterProcessStart(qlName);
                    Process.Start(qlPath);
                }
            }
        }

        private void RadioRedTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Red";
            SaveAndSetTheme(_appTheme);
        }

        private void RadioOrangeTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Orange";
            SaveAndSetTheme(_appTheme);
        }

        private void RadioBlueTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Blue";
            SaveAndSetTheme(_appTheme);
        }

        private void RadioPurpleTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Purple";
            SaveAndSetTheme(_appTheme);
        }

        private void RadioGreenTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Green";
            SaveAndSetTheme(_appTheme);
        }

        private void RadioTealTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Teal";
            SaveAndSetTheme(_appTheme);
        }

        // set chosen theme and save to .ini file
        private void SaveAndSetTheme(string themeName)
        {
            _appTheme = themeName;
            ViewUpdater.SetAppTheme(this, _appTheme);
            ConfigManager.IniWrite("AppTheme", _appTheme);
        }

        private void AudioDevice1_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string audioDeviceSwitch1 = AudioDevice1.SelectedValue.ToString();
            ConfigManager.IniWrite("AudioDevice1", audioDeviceSwitch1);
            ReadSettings();
        }

        private void AudioDevice2_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string audioDeviceSwitch2 = AudioDevice2.SelectedValue.ToString();
            ConfigManager.IniWrite("AudioDevice2", audioDeviceSwitch2);
            ReadSettings();
        }

        private void GameModeHotKey_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string gameModeHotKeyValue = GameModeHotKey.SelectedValue.ToString();
            if (gameModeHotKeyValue != _audioDeviceSwitchHotKey && gameModeHotKeyValue != _exitAppHotKey)
            {
                ConfigManager.IniWrite("GameModeHotKey", gameModeHotKeyValue);
                ReadSettings();
            }
            else
            {
                ShowMetroMessage("HotKey already assigned.", "Choose another HotKey for this task");
            }
        }

        private void AudioDeviceHotKey_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string audioDeviceHotKeyValue = AudioDeviceHotKey.SelectedValue.ToString();

            if (audioDeviceHotKeyValue != _gameModeHotKey && audioDeviceHotKeyValue != _exitAppHotKey)
            {
                ConfigManager.IniWrite("AudioDeviceSwitchHotKey", audioDeviceHotKeyValue);
                ReadSettings();
            }
            else
            {
                ShowMetroMessage("HotKey already assigned.", "Choose another HotKey for this task");
            }
        }

        private void ExitAppHotKey_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string exitAppHotKeyValue = ExitAppHotKey.SelectedValue.ToString();

            if (exitAppHotKeyValue != _gameModeHotKey && exitAppHotKeyValue != _audioDeviceSwitchHotKey)
            {
                ConfigManager.IniWrite("ExitAppHotKey", exitAppHotKeyValue);
                ReadSettings();
            }
            else
            {
                ShowMetroMessage("HotKey already assigned.", "Choose another HotKey for this task");
            }
        }

        private void HotKeyManager(object sender, KeyEventArgs e)
        {
            string keyPress = e.Key.ToString();
            double currentTimer = ServiceManager.CurrentTimerRes() / 10000.0;

            if (GridSettings.Visibility == Visibility.Collapsed)
            {
                if (keyPress.Equals(_gameModeHotKey))
                {
                    switch (PowerPlanBadge.Badge.ToString())
                    {
                        case " High Performance " when currentTimer > 0.6:
                            TimerResolutionTile_OnClick(sender, e);
                            break;

                        case " Balanced " when currentTimer < 0.6:
                            PowerPlanTile_OnCLick(sender, e);
                            break;

                        default:
                            TimerResolutionTile_OnClick(sender, e);
                            PowerPlanTile_OnCLick(sender, e);
                            break;
                    }
                }
                if (keyPress.Equals(_audioDeviceSwitchHotKey))
                {
                    AudioDeviceSwitchTile_OnClick(sender, e);
                }
                if (keyPress.Equals(_exitAppHotKey))
                {
                    MainWindow_Closed(sender, e);
                }
            }
        }

        private void InstallServiceButton_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceManager.MiTLService.Refresh();
            ServiceManager.InstallService();
            InstallServiceButton.IsEnabled = false;
            UninstallServiceButton.IsEnabled = true;
        }

        private void UninstallServiceButton_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceManager.MiTLService.Refresh();
            ServiceManager.UninstallService();
            InstallServiceButton.IsEnabled = true;
            UninstallServiceButton.IsEnabled = false;
        }

        //reflect updated settings in UI when window is re-focused
        private void MainWindow_Activated(object sender, EventArgs e)
        {
            ReadSettings();
            ShowActivePowerPlan();
            ShowDefaultAudioDevice();
            SetComboListIndexes();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //close hardware monitoring when exiting
            SystemMonitor.ThisPc.Close();
            Close();
        }

        private void ShowMetroMessage(string title, string message)
        {
            if (Application.Current.MainWindow is MetroWindow metroWindow)
            {
                metroWindow.ShowMessageAsync(title, message);
            }
        }

        private void NavMonitoring_Click(object sender, RoutedEventArgs e)
        {
            _viewGridName = GridMonitoring;
            _viewIndicatorName = NavMonitoringIndicator;
            NavUpdateView();
        }

        private void NavPerformance_Click(object sender, RoutedEventArgs e)
        {
            if (_closeAfterburner == "true")
            {
                ToggleCloseAfterburner.IsChecked = true;
            }
            else
            {
                ToggleCloseAfterburner.IsChecked = false;
            }

            _viewGridName = GridPerformance;
            _viewIndicatorName = NavPerformanceIndicator;
            NavUpdateView();
        }

        private void NavQuickLaunch_Click(object sender, RoutedEventArgs e)
        {
            ShowQuicklaunchTiles();
            _viewGridName = GridQuickLaunch;
            _viewIndicatorName = NavQuickLaunchIndicator;
            NavUpdateView();
        }

        private void NavSettings_Click(object sender, RoutedEventArgs e)
        {
            _viewGridName = GridSettings;
            _viewIndicatorName = NavSettingsIndicator;
            NavUpdateView();

            foreach (Image qlTileImage in ListManager.QuicklaunchTileImages)
            {
                qlTileImage.Source = null;
            }

            foreach (Tile qlTiles in ListManager.QuicklaunchTiles)
            {
                qlTiles.Title = null;
            }
        }

        private void NavAbout_Click(object sender, RoutedEventArgs e)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string fileVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
            VersionNumber.Content = "v" + fileVersion;
            _viewGridName = GridAbout;
            _viewIndicatorName = NavAboutIndicator;
            NavUpdateView();
        }

        private void NavUpdateView()
        {
            foreach (Border viewIndicator in ListManager.NavigationIndicators)
            {
                viewIndicator.Visibility = Visibility.Collapsed;
            }

            foreach (Grid viewGrid in ListManager.NavigationGrids)
            {
                viewGrid.Visibility = Visibility.Collapsed;
            }

            _viewIndicatorName.Visibility = Visibility.Visible;
            _viewGridName.Visibility = Visibility.Visible;
        }

        private void SettingsTab_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ReadSettings();

            Label senderLabel = (Label)sender;
            string tabName = senderLabel.ToString();

            foreach (Border tabIndicator in ListManager.SettingsTabIndicators)
            {
                tabIndicator.Visibility = Visibility.Collapsed;
            }

            if (tabName.Contains("Timer Service"))
            {
                SettingsTimerServiceindicator.Visibility = Visibility.Visible;
            }
            if (tabName.Contains("Audio Devices"))
            {
                SettingsAudioDevicesindicator.Visibility = Visibility.Visible;
            }
            if (tabName.Contains("Quick Launch"))
            {
                SettingsQuickLaunchindicator.Visibility = Visibility.Visible;

                int index = 0;

                foreach (TextBox item in ListManager.QuicklaunchTexBoxes)
                {
                    item.Text = QlNames[index];
                    index++;
                }
            }
            if (tabName.Contains("Hotkeys"))
            {
                SettingsHotkeysindicator.Visibility = Visibility.Visible;
            }
            if (tabName.Contains("Theme"))
            {
                SettingsThemeindicator.Visibility = Visibility.Visible;

                Theme currentTheme = ThemeManager.Current.DetectTheme(this);
                if (currentTheme != null && currentTheme.Name == "Dark.Red")
                {
                    RadioRedTheme.IsChecked = true;
                }
                if (currentTheme != null && currentTheme.Name == "Dark.Orange")
                {
                    RadioOrangeTheme.IsChecked = true;
                }
                if (currentTheme != null && currentTheme.Name == "Dark.Blue")
                {
                    RadioBlueTheme.IsChecked = true;
                }
                if (currentTheme != null && currentTheme.Name == "Dark.Purple")
                {
                    RadioPurpleTheme.IsChecked = true;
                }
                if (currentTheme != null && currentTheme.Name == "Dark.Green")
                {
                    RadioGreenTheme.IsChecked = true;
                }
                if (currentTheme != null && currentTheme.Name == "Dark.Teal")
                {
                    RadioTealTheme.IsChecked = true;
                }
            }
        }

        private void Filepicker_OnClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button SenderButton)
            {
                qlTileNumber = SenderButton.Tag.ToString();
            }

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"C:\",
                Title = "Browse EXE Files",
                DefaultExt = "exe",
                Filter = "exe files (*.exe)|*.exe",
                FilterIndex = 2,
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true,
                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fullFilename = openFileDialog.FileName;
                string friendlyFileName = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName);
                string configPath = "Quicklaunch" + qlTileNumber + "Path";
                string configName = "Quicklaunch" + qlTileNumber + "Name";
                ConfigManager.IniWrite(configPath, fullFilename);
                ConfigManager.IniWrite(configName, friendlyFileName);
                string labelName = "QlTextBox" + qlTileNumber;
                TextBox qlTextBox = (TextBox)FindName(labelName);
                qlTextBox.Text = friendlyFileName;
                IconManager.GrabAndSaveIcon(qlTileNumber, fullFilename);
                ReadSettings();
            }
        }

        private void QlResetTile(object sender, RoutedEventArgs e)
        {
            if (sender is Button SenderButton)
            {
                qlTileNumber = SenderButton.Tag.ToString();
            }

            string messageString = "Reset Quicklaunch Tile " + qlTileNumber + " ?";
            if (MessageBox.Show(messageString, "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string configPath = "Quicklaunch" + qlTileNumber + "Path";
                string configName = "Quicklaunch" + qlTileNumber + "Name";
                string iconPath = Environment.CurrentDirectory + @"\ql_icons\";
                string iconFile = iconPath + "ql" + qlTileNumber + ".ico";
                ConfigManager.IniWrite(configPath, "");
                ConfigManager.IniWrite(configName, "");

                if (File.Exists(iconFile))
                {
                    File.Delete(iconFile);
                }

                int qlindexNumber = int.Parse(qlTileNumber) - 1;
                string qlPath = QlPaths[qlindexNumber];
                TextBox qlTexbBox = ListManager.QuicklaunchTexBoxes[qlindexNumber];

                if (qlPath.Length > 1)
                {
                    qlTexbBox.Text = "";
                }
            }
        }

        private void QlNameEnter(object sender, KeyEventArgs e)
        {
            if (sender is TextBox SenderTextBox)
            {
                qlTileNumber = SenderTextBox.Tag.ToString();
            }

            string configName = "Quicklaunch" + qlTileNumber + "Name";
            if (e.Key == Key.Enter)
            {
                string configValue;

                int qlindexNumber = int.Parse(qlTileNumber) - 1;
                string qlPath = QlPaths[qlindexNumber];
                TextBox qlTexbBox = ListManager.QuicklaunchTexBoxes[qlindexNumber];

                if (qlPath.Length > 1)
                {
                    configValue = qlTexbBox.Text;
                    ConfigManager.IniWrite(configName, configValue);
                }
            }
        }

        private List<string> QlNames
        {
            get
            {
                List<string> qlNames = new List<string>
                {
                    _quicklaunch1Name,
                    _quicklaunch2Name,
                    _quicklaunch3Name,
                    _quicklaunch4Name,
                    _quicklaunch5Name,
                    _quicklaunch6Name,
                    _quicklaunch7Name,
                    _quicklaunch8Name,
                    _quicklaunch9Name,
                    _quicklaunch10Name,
                    _quicklaunch11Name,
                    _quicklaunch12Name,
                    _quicklaunch13Name,
                    _quicklaunch14Name,
                    _quicklaunch15Name,
                    _quicklaunch16Name
                };
                return qlNames;
            }
        }

        private List<string> QlPaths
        {
            get
            {
                List<string> qlPaths = new List<string>
                {
                    _quicklaunch1Path,
                    _quicklaunch2Path,
                    _quicklaunch3Path,
                    _quicklaunch4Path,
                    _quicklaunch5Path,
                    _quicklaunch6Path,
                    _quicklaunch7Path,
                    _quicklaunch8Path,
                    _quicklaunch9Path,
                    _quicklaunch10Path,
                    _quicklaunch11Path,
                    _quicklaunch12Path,
                    _quicklaunch13Path,
                    _quicklaunch14Path,
                    _quicklaunch15Path,
                    _quicklaunch16Path
                };
                return qlPaths;
            }
        }
    }
}