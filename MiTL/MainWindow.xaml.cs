using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MiTL
{
    public partial class MainWindow
    {
        private static readonly ListManager ListManager = new ListManager();
        private static readonly ConfigManager ConfigManager = new ConfigManager();
        private static Grid _settingsGridName = new Grid();
        private static Tile _settingsTileName = new Tile();
        private static string _settingsPageTitle = "";
        private static string _activePowerPlan;
        private static string _powerPlanBalanced;
        private static string _powerPlanPerformance;
        private static string _isMonitoringEnabled;
        private static string _appTheme;
        private static string _gameModeHotKey;
        private static string _audioDeviceSwitchHotKey;
        private static string _exitAppHotKey;
        private static string _audioDevice1;
        private static string _audioDevice2;
        private static string _defaultAudioDevice;

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

            //start Timer Resolution Monitoring
            StartTimerMonitor();

            //check if system monitoring is enabled and start if wanted
            IsMonitorEnabled();
        }

        //app startup checks
        private void Startup()
        {
            //check for duplicate process instances
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location)).Length > 1)
            {
                Process.GetCurrentProcess().Kill();
            }

            //check if config file exists, if not write default config file
            string myConfigManager = Properties.Resources.MyConfigManager;
            if (!File.Exists(myConfigManager))
            {
                ConfigManager.WriteConfigDefaults();
                ReadSettings();
            }
            else
            {
                ReadSettings();
            }

            //set app theme
            ViewUpdater.SetAppTheme(this, _appTheme);
        }

        //read in values from ini file
        private static void ReadSettings()
        {
            _powerPlanBalanced = ConfigManager.IniRead("PowerPlanBalanced");
            _powerPlanPerformance = ConfigManager.IniRead("PowerPlanPerformance");
            _isMonitoringEnabled = ConfigManager.IniRead("IsMonitoringEnabled");
            _appTheme = ConfigManager.IniRead("AppTheme");
            _gameModeHotKey = ConfigManager.IniRead("GameModeHotKey");
            _audioDeviceSwitchHotKey = ConfigManager.IniRead("AudioDeviceSwitchHotKey");
            _exitAppHotKey = ConfigManager.IniRead("ExitAppHotKey");
            _audioDevice1 = ConfigManager.IniRead("AudioDevice1");
            _audioDevice2 = ConfigManager.IniRead("AudioDevice2");
            _defaultAudioDevice = ConfigManager.IniRead("DefaultAudioDevice");
        }

        //set if monitoring is enabled and show if true
        private void IsMonitorEnabled()
        {
            switch (_isMonitoringEnabled)
            {
                case "0":
                    HideMonitoring();
                    CheckboxEnableMonitor.IsOn = false;
                    break;

                case "1":
                    ShowMonitoring();
                    CheckboxEnableMonitor.IsOn = true;
                    StartHardwareMonitor();
                    break;

                default:
                    break;
            }
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
                _activePowerPlan = " " + "Balanced Profile" + " ";
            }
            if (activePlan.Contains(_powerPlanPerformance))
            {
                _activePowerPlan = " " + "High Performance Profile" + " ";
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
                AudioDeviceBadge.Badge = " " + _defaultAudioDevice + " ";
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
                case " Balanced Profile ":
                    ApplyPowerPlan(_powerPlanPerformance);
                    break;

                case " High Performance Profile ":
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

        private void ToggleSwitch_MonitoringOnOff(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleSwitch toggleSwitch)
            {
                switch (toggleSwitch.IsOn)
                {
                    case true:
                        ConfigManager.IniWrite("IsMonitoringEnabled", "1");
                        ShowMonitoring();
                        SystemMonitor.StartHardwareMonitor();
                        break;

                    default:
                        ConfigManager.IniWrite("IsMonitoringEnabled", "0");
                        SystemMonitor.ThisPc.Close();
                        HideMonitoring();
                        break;
                }
            }
        }

        private static void ShowMonitoring()
        {
            MonitoringDecision(ListManager.ControlsFrames, ListManager.ControlsLabels, "Enable");
        }

        private static void HideMonitoring()
        {
            MonitoringDecision(ListManager.ControlsFrames, ListManager.ControlsLabels, "Disable");
        }

        private static void MonitoringDecision(IEnumerable<Frame> controlsFrames, IEnumerable<Label> controlsLabels, string decision)
        {
            if (controlsFrames == null)
            {
                throw new ArgumentNullException(nameof(controlsFrames));
            }

            if (controlsLabels == null)
            {
                throw new ArgumentNullException(nameof(controlsLabels));
            }

            foreach (Frame monitorFrames in controlsFrames)
            {
                switch (decision)
                {
                    case "Enable":
                        monitorFrames.Opacity = 1;
                        break;

                    case "Disable":
                        monitorFrames.Opacity = 0.6;
                        break;

                    default:
                        break;
                }
            }

            foreach (Label monitorLabels in controlsLabels)
            {
                switch (decision)
                {
                    case "Enable":
                        monitorLabels.Visibility = Visibility.Visible;
                        break;

                    case "Disable":
                        monitorLabels.Content = null;
                        monitorLabels.Visibility = Visibility.Hidden;
                        break;

                    default:
                        break;
                }
            }
        }

        private void RadioRedTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Red";
            SaveAndSetTheme(_appTheme);
        }

        private void RadioAmberTheme_OnChecked(object sender, RoutedEventArgs e)
        {
            _appTheme = "Dark.Amber";
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
        }

        private void AudioDevice2_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string audioDeviceSwitch2 = AudioDevice2.SelectedValue.ToString();
            ConfigManager.IniWrite("AudioDevice2", audioDeviceSwitch2);
        }

        private void GameModeHotKey_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            string gameModeHotKeyValue = GameModeHotKey.SelectedValue.ToString();
            if (gameModeHotKeyValue != _audioDeviceSwitchHotKey && gameModeHotKeyValue != _exitAppHotKey)
            {
                ConfigManager.IniWrite("GameModeHotKey", gameModeHotKeyValue);
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

            if (keyPress.Equals(_gameModeHotKey))
            {
                switch (PowerPlanBadge.Badge.ToString())
                {
                    case " High Performance Profile " when currentTimer > 0.6:
                        TimerResolutionTile_OnClick(sender, e);
                        break;

                    case " Balanced Profile " when currentTimer < 0.6:
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
            if (keyPress.Equals("Escape"))
            {
                if (SettingsFlyout.IsOpen)
                {
                    SettingsFlyout.IsOpen = false;
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

        private void FlyoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SettingsFlyout.IsOpen == false)
            {
                Theme currentTheme = ThemeManager.Current.DetectTheme(this);
                if (currentTheme != null && currentTheme.Name == "Dark.Red")
                {
                    RadioRedTheme.IsChecked = true;
                }
                if (currentTheme != null && currentTheme.Name == "Dark.Amber")
                {
                    RadioAmberTheme.IsChecked = true;
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

                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                string fileVersion = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
                VersionNumber.Content = "v" + fileVersion;

                SettingsFlyout.IsOpen = true;
            }
        }

        private void FlyoutButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            FlyoutButton.BorderBrush = Brushes.White;
        }

        private void FlyoutButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (FlyoutButton.BorderBrush == Brushes.White)
            {
                FlyoutButton.BorderBrush = null;
            }
        }

        private void SettingsFlyout_OnClosingFinished(object sender, RoutedEventArgs e)
        {
            ReadSettings();
        }

        private void SettingsThemeTile_OnClick(object sender, RoutedEventArgs e)
        {
            _settingsGridName = SettingsThemeGrid;
            _settingsTileName = SettingsThemeTile;
            _settingsPageTitle = "Theme Changer";
            UpdateSettingsView();
        }

        private void SettingsMonitoringTile_OnClick(object sender, RoutedEventArgs e)
        {
            _settingsGridName = SettingsMonitoringGrid;
            _settingsTileName = SettingsMonitoringTile;
            _settingsPageTitle = "Real-time System Monitoring";
            UpdateSettingsView();
        }

        private void SettingsTimerServiceTile_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceManager.MiTLService.Refresh();
            if (ServiceManager.ServiceExists())
            {
                InstallServiceButton.IsEnabled = false;
                UninstallServiceButton.IsEnabled = true;
            }
            else
            {
                InstallServiceButton.IsEnabled = true;
                UninstallServiceButton.IsEnabled = false;
            }

            _settingsGridName = SettingsTimerServiceGrid;
            _settingsTileName = SettingsTimerServiceTile;
            _settingsPageTitle = "Windows Timer Resolution Service";
            UpdateSettingsView();
        }

        private void SettingsAudioOutputsTile_OnClick(object sender, RoutedEventArgs e)
        {
            _settingsGridName = SettingsAudioOutputsGrid;
            _settingsTileName = SettingsAudioOutputsTile;
            _settingsPageTitle = "Audio Output Devices Selection";
            UpdateSettingsView();
        }

        private void SettingsQuickLaunchTile_OnClick(object sender, RoutedEventArgs e)
        {
            _settingsGridName = SettingsQuickLaunchGrid;
            _settingsTileName = SettingsQuickLaunchTile;
            _settingsPageTitle = "Quick Launch Shortcuts";
            UpdateSettingsView();
        }

        private void SettingsHotKeysTile_OnClick(object sender, RoutedEventArgs e)
        {
            _settingsGridName = SettingsHotKeysGrid;
            _settingsTileName = SettingsHotKeysTile;
            _settingsPageTitle = "HotKey Binding Configuration";
            UpdateSettingsView();
        }

        private void UpdateSettingsView()
        {
            foreach (Grid settingsGrid in ListManager.SettingsGrids)
            {
                settingsGrid.Visibility = Visibility.Collapsed;
            }

            foreach (Tile settingsTile in ListManager.SettingsTiles)
            {
                settingsTile.BorderThickness = new Thickness(0);
            }

            _settingsGridName.Visibility = Visibility.Visible;
            _settingsTileName.BorderThickness = new Thickness(2);
            SettingsPageTitle.Text = _settingsPageTitle;
        }

        private void ShowMetroMessage(string title, string message)
        {
            if (Application.Current.MainWindow is MetroWindow metroWindow)
            {
                metroWindow.ShowMessageAsync(title, message);
            }
        }
    }
}