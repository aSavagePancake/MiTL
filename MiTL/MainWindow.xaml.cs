using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        private static readonly RegistryManager RegistryManager = new RegistryManager();
        private static Grid _viewGridName = new Grid();
        private static Border _viewIndicatorName = new Border();
        private SolidColorBrush accentColor;
        private static string _powerPlanBalanced;
        private static string _powerPlanPerformance;
        private static string _appTheme;
        private static string _closeOnLaunch;
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
            ShowGameModeStatus();
            ShowCurrentTimer();

            //start Monitoring
            StartHardwareMonitor();
        }

        //app startup checks
        private void Startup()
        {
            //check for duplicate process instances
            ProcessManager.TerminateDuplicate();

            //check if config file exists, if not write default config file
            string SettingsFile = Properties.Resources.SettingsFile;
            if (!File.Exists(SettingsFile))
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
            _powerPlanBalanced = ConfigManager.IniRead("PowerPlanBalanced");
            _powerPlanPerformance = ConfigManager.IniRead("PowerPlanPerformance");
            _appTheme = ConfigManager.IniRead("AppTheme");
            _closeOnLaunch = ConfigManager.IniRead("CloseOnLaunch");
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

            accentColor = TryFindResource("MahApps.Brushes.Accent") as SolidColorBrush;

            if (activePlan.Contains(_powerPlanBalanced))
            {
                PowerPlanBalancedTile.Background = accentColor;
                PowerPlanPerformanceTile.Background = Brushes.Transparent;
            }
            if (activePlan.Contains(_powerPlanPerformance))
            {
                PowerPlanPerformanceTile.Background = accentColor;
                PowerPlanBalancedTile.Background = Brushes.Transparent;
            }
        }

        private void ShowGameModeStatus()
        {
            accentColor = TryFindResource("MahApps.Brushes.Accent") as SolidColorBrush;

            string subKey = Properties.Resources.RegGameModeSubkey;
            string key = Properties.Resources.RegGameModeKey;
            string gameModeStatus = RegistryManager.RegKeyRead(subKey, key);

            if (gameModeStatus == "0")
            {
                GameModeDisableTile.Background = accentColor;
                GameModeEnableTile.Background = Brushes.Transparent;
            }
            else
            {
                GameModeEnableTile.Background = accentColor;
                GameModeDisableTile.Background = Brushes.Transparent;
            }
        }

        private void ShowCurrentTimer()
        {
            accentColor = TryFindResource("MahApps.Brushes.Accent") as SolidColorBrush;

            ServiceManager.MiTLService.Refresh();

            if (ServiceManager.ServiceExists())
            {
                if (ServiceManager.ServiceRunning())
                {
                    PerformanceTimerTile.Background = accentColor;
                    DefaultTimerTile.Background = Brushes.Transparent;
                }
                else
                {
                    DefaultTimerTile.Background = accentColor;
                    PerformanceTimerTile.Background = Brushes.Transparent;
                }
            }
            else
            {
                DefaultTimerTile.Background = accentColor;
                PerformanceTimerTile.Background = Brushes.Transparent;
            }
        }

        private void ShowQuicklaunchTiles()
        {
            string iconFolder = Environment.CurrentDirectory + @"\ql_icons\";
            string iconExtension = ".ico";

            List<string> qlIconNames = new List<string>
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

            foreach (string qlIconName in qlIconNames)
            {
                string qlTileNumber = qlIconName;
                int index = qlIconNames.IndexOf(qlTileNumber);

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

        public void StartHardwareMonitor()
        {
            SystemMonitor.StartHardwareMonitor();
        }

        private void PowerPlanBalancedTile_OnCLick(object sender, RoutedEventArgs e)
        {
            ApplyPowerPlan(_powerPlanBalanced);
        }

        private void PowerPlanPerformanceTile_OnCLick(object sender, RoutedEventArgs e)
        {
            ApplyPowerPlan(_powerPlanPerformance);
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

        private void GameModeDisableTile_OnClick(object sender, RoutedEventArgs e)
        {
            string subKey = Properties.Resources.RegGameModeSubkey;
            string key = Properties.Resources.RegGameModeKey;
            string value = "0";
            RegistryValueKind kind = RegistryValueKind.DWord;
            RegistryManager.RegKeyWrite(subKey, key, value, kind);
            ShowGameModeStatus();
        }

        private void GameModeEnableTile_OnClick(object sender, RoutedEventArgs e)
        {
            string subKey = Properties.Resources.RegGameModeSubkey;
            string key = Properties.Resources.RegGameModeKey;
            string value = "1";
            RegistryValueKind kind = RegistryValueKind.DWord;
            RegistryManager.RegKeyWrite(subKey, key, value, kind);
            ShowGameModeStatus();
        }

        private void PerformanceTimerTile_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceManager.MiTLService.Refresh();
            if (!ServiceManager.ServiceExists())
            {
                ShowMetroMessage("Service Not Found.", "To change this, Install the Timer Resolution Service via Settings Panel");
            }
            else
            {
                ServiceManager.StartService();
                accentColor = TryFindResource("MahApps.Brushes.Accent") as SolidColorBrush;
                PerformanceTimerTile.Background = accentColor;
                DefaultTimerTile.Background = Brushes.Transparent;
            }
        }

        private void DefaultTimerTile_OnClick(object sender, RoutedEventArgs e)
        {
            ServiceManager.MiTLService.Refresh();
            if (!ServiceManager.ServiceExists())
            {
                return;
            }
            else
            {
                if (ServiceManager.ServiceRunning())
                {
                    ServiceManager.StopService();
                }

                ServiceManager.MiTLService.Refresh();
                ShowCurrentTimer();
            }
        }

        private void GpuProfile1Tile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("1");
        }

        private void GpuProfile2Tile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("2");
        }

        private void GpuProfile3Tile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("3");
        }

        private void GpuProfile4Tile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("4");
        }

        private void GpuProfile5Tile_Click(object sender, RoutedEventArgs e)
        {
            ApplyProfile("5");
        }

        //Apply selected profile
        private void ApplyProfile(string profile)
        {
            string args = "/Profile" + profile + " /q";

            //start MSIAfterburner using appropriate /profile switch and quit immediately using /q switch
            string _msiabFile = Properties.Resources.MSIAB_FilePath;
            Process.Start(_msiabFile, args);
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
                    if (ToggleCloseOnLaunch.IsChecked == true)
                    {
                        ProcessManager.CloseAfterProcessStart(qlName);
                    }
                    Process.Start(qlPath);
                }
            }
        }

        private void ToggleCloseOnLaunch_Checked(object sender, RoutedEventArgs e)
        {
            ConfigManager.IniWrite("CloseOnLaunch", "true");
        }

        private void ToggleCloseOnLaunch_Unchecked(object sender, RoutedEventArgs e)
        {
            ConfigManager.IniWrite("CloseOnLaunch", "false");
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
            ShowGameModeStatus();
            ShowCurrentTimer();
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
            ReadSettings();
            _viewGridName = GridPerformance;
            _viewIndicatorName = NavPerformanceIndicator;
            ShowActivePowerPlan();
            ShowGameModeStatus();
            ShowCurrentTimer();
            NavUpdateView();
        }

        private void NavLauncher_Click(object sender, RoutedEventArgs e)
        {
            ReadSettings();

            switch (_closeOnLaunch)
            {
                case "true":
                    ToggleCloseOnLaunch.IsChecked = true;
                    break;

                default:
                    ToggleCloseOnLaunch.IsChecked = false;
                    break;
            }

            ShowQuicklaunchTiles();

            _viewGridName = GridLauncher;
            _viewIndicatorName = NavLauncherIndicator;
            NavUpdateView();
        }

        private void NavOSTweaks_Click(object sender, RoutedEventArgs e)
        {
            _viewGridName = GridOSTweaks;
            _viewIndicatorName = NavOSTweaksIndicator;
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
            foreach (Border navIndicator in ListManager.NavigationIndicators)
            {
                navIndicator.Visibility = Visibility.Collapsed;
            }

            foreach (Grid navGrid in ListManager.NavigationGrids)
            {
                navGrid.Visibility = Visibility.Collapsed;
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
                SettingsLauncherindicator.Visibility = Visibility.Visible;

                int index = 0;

                foreach (TextBox qlTextBox in ListManager.QuicklaunchTexBoxes)
                {
                    qlTextBox.Text = QlNames[index];
                    index++;
                }
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