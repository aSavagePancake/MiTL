using ControlzEx.Theming;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MiTL
{
    public partial class MainWindow
    {
        private static readonly ConfigManager ConfigManager = new ConfigManager();
        private static readonly RegistryManager RegistryManager = new RegistryManager();
        private static Grid _viewGridName = new Grid();
        private static Border _viewIndicatorName = new Border();
        private SolidColorBrush accentColor;
        private static string _powerPlanBalanced;
        private static string _powerPlanPerformance;
        private static string _appTheme;
        private static string _closeOnLaunch;
        private static string _launcher1Name;
        private static string _launcher1Path;
        private static string _launcher2Name;
        private static string _launcher2Path;
        private static string _launcher3Name;
        private static string _launcher3Path;
        private static string _launcher4Name;
        private static string _launcher4Path;
        private static string _launcher5Name;
        private static string _launcher5Path;
        private static string _launcher6Name;
        private static string _launcher6Path;
        private static string _launcher7Name;
        private static string _launcher7Path;
        private static string _launcher8Name;
        private static string _launcher8Path;
        private static string _launcher9Name;
        private static string _launcher9Path;
        private static string _launcher10Name;
        private static string _launcher10Path;
        private static string _launcher11Name;
        private static string _launcher11Path;
        private static string _launcher12Name;
        private static string _launcher12Path;
        private static string _launcher13Name;
        private static string _launcher13Path;
        private static string _launcher14Name;
        private static string _launcher14Path;
        private static string _launcher15Name;
        private static string _launcher15Path;
        private static string _launcher16Name;
        private static string _launcher16Path;
        private static string launcherTileNumber = "";

        [DllImport("PowrProf.dll")]
        private static extern uint PowerEnumerate(IntPtr rootPowerKey, IntPtr schemeGuid,
            IntPtr subGroupOfPowerSettingGuid, uint accessFlags, uint index, ref Guid buffer, ref uint bufferSize);

        [DllImport("PowrProf.dll")]
        private static extern uint PowerReadFriendlyName(IntPtr rootPowerKey, ref Guid schemeGuid,
            IntPtr subGroupOfPowerSettingGuid, IntPtr powerSettingGuid, IntPtr buffer, ref uint bufferSize);

        private enum AccessFlags : uint
        {
            AccessScheme = 16,
        }

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
            _launcher1Name = ConfigManager.IniRead("Launcher1Name");
            _launcher1Path = ConfigManager.IniRead("Launcher1Path");
            _launcher2Name = ConfigManager.IniRead("Launcher2Name");
            _launcher2Path = ConfigManager.IniRead("Launcher2Path");
            _launcher3Name = ConfigManager.IniRead("Launcher3Name");
            _launcher3Path = ConfigManager.IniRead("Launcher3Path");
            _launcher4Name = ConfigManager.IniRead("Launcher4Name");
            _launcher4Path = ConfigManager.IniRead("Launcher4Path");
            _launcher5Name = ConfigManager.IniRead("Launcher5Name");
            _launcher5Path = ConfigManager.IniRead("Launcher5Path");
            _launcher6Name = ConfigManager.IniRead("Launcher6Name");
            _launcher6Path = ConfigManager.IniRead("Launcher6Path");
            _launcher7Name = ConfigManager.IniRead("Launcher7Name");
            _launcher7Path = ConfigManager.IniRead("Launcher7Path");
            _launcher8Name = ConfigManager.IniRead("Launcher8Name");
            _launcher8Path = ConfigManager.IniRead("Launcher8Path");
            _launcher9Name = ConfigManager.IniRead("Launcher9Name");
            _launcher9Path = ConfigManager.IniRead("Launcher9Path");
            _launcher10Name = ConfigManager.IniRead("Launcher10Name");
            _launcher10Path = ConfigManager.IniRead("Launcher10Path");
            _launcher11Name = ConfigManager.IniRead("Launcher11Name");
            _launcher11Path = ConfigManager.IniRead("Launcher11Path");
            _launcher12Name = ConfigManager.IniRead("Launcher12Name");
            _launcher12Path = ConfigManager.IniRead("Launcher12Path");
            _launcher13Name = ConfigManager.IniRead("Launcher13Name");
            _launcher13Path = ConfigManager.IniRead("Launcher13Path");
            _launcher14Name = ConfigManager.IniRead("Launcher14Name");
            _launcher14Path = ConfigManager.IniRead("Launcher14Path");
            _launcher15Name = ConfigManager.IniRead("Launcher15Name");
            _launcher15Path = ConfigManager.IniRead("Launcher15Path");
            _launcher16Name = ConfigManager.IniRead("Launcher16Name");
            _launcher16Path = ConfigManager.IniRead("Launcher16Path");
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

        private void ShowLauncherTiles()
        {
            string iconFolder = Environment.CurrentDirectory + @"\launcher\";
            string iconExtension = ".ico";

            List<string> launcherIconNames = new List<string>
            {
                "launcher1",
                "launcher2",
                "launcher3",
                "launcher4",
                "launcher5",
                "launcher6",
                "launcher7",
                "launcher8",
                "launcher9",
                "launcher10",
                "launcher11",
                "launcher12",
                "launcher13",
                "launcher14",
                "launcher15",
                "launcher16"
            };

            foreach (string launcherIconName in launcherIconNames)
            {
                string launcherTileNumber = launcherIconName;
                int index = launcherIconNames.IndexOf(launcherTileNumber);

                string launcherName = LauncherNames[index];
                Tile launcherTile = LauncherTiles[index];
                Image launcherImage = LauncherTileImages[index];

                string iconPath = iconFolder + launcherTileNumber + iconExtension;

                if (launcherName.Length > 1)
                {
                    launcherImage.Source = BitmapFromFile(iconPath);
                    launcherTile.Title = launcherName;
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
            //start MSIAfterburner using appropriate /profile switch and quit immediately using /q switch
            string args = "/Profile" + profile + " /q";
            string _msiabFile = Properties.Resources.MSIAB_FilePath;
            Process.Start(_msiabFile, args);
        }

        private void LauncherTile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Tile SenderTile)
            {
                launcherTileNumber = SenderTile.Tag.ToString();
            }

            int launcherindexNumber = int.Parse(launcherTileNumber) - 1;
            string launcherPath = LauncherPaths[launcherindexNumber];
            string launcherExeName = Path.GetFileNameWithoutExtension(launcherPath);

            if (launcherExeName.Length > 1)
            {
                if (File.Exists(launcherPath))
                {
                    if (ToggleCloseOnLaunch.IsChecked == true)
                    {
                        ProcessManager.CloseAfterProcessStart(launcherExeName);
                    }
                    Process.Start(launcherPath);
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

            ShowLauncherTiles();

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

            foreach (Image launcherTileImage in LauncherTileImages)
            {
                launcherTileImage.Source = null;
            }

            foreach (Tile launcherTiles in LauncherTiles)
            {
                launcherTiles.Title = null;
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
            foreach (Border navIndicator in NavigationIndicators)
            {
                navIndicator.Visibility = Visibility.Collapsed;
            }

            foreach (Grid navGrid in NavigationGrids)
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

            foreach (Border tabIndicator in SettingsTabIndicators)
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
            if (tabName.Contains("Launcher"))
            {
                SettingsLauncherindicator.Visibility = Visibility.Visible;

                int index = 0;

                foreach (TextBox launcherTextBox in LauncherTexBoxes)
                {
                    launcherTextBox.Text = LauncherNames[index];
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
                launcherTileNumber = SenderButton.Tag.ToString();
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
                string configPath = "Launcher" + launcherTileNumber + "Path";
                string configName = "Launcher" + launcherTileNumber + "Name";
                ConfigManager.IniWrite(configPath, fullFilename);
                ConfigManager.IniWrite(configName, friendlyFileName);
                string labelName = "LauncherTextBox" + launcherTileNumber;
                TextBox launcherTextBox = (TextBox)FindName(labelName);
                launcherTextBox.Text = friendlyFileName;
                IconManager.GrabAndSaveIcon(launcherTileNumber, fullFilename);
                ReadSettings();
            }
        }

        private void LauncherResetTile(object sender, RoutedEventArgs e)
        {
            if (sender is Button SenderButton)
            {
                launcherTileNumber = SenderButton.Tag.ToString();
            }

            string messageString = "Reset Launcher Tile " + launcherTileNumber + " ?";
            if (MessageBox.Show(messageString, "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                string configPath = "Launcher" + launcherTileNumber + "Path";
                string configName = "Launcher" + launcherTileNumber + "Name";
                string iconPath = Environment.CurrentDirectory + @"\launcher\";
                string iconFile = iconPath + "launcher" + launcherTileNumber + ".ico";
                ConfigManager.IniWrite(configPath, "");
                ConfigManager.IniWrite(configName, "");

                if (File.Exists(iconFile))
                {
                    File.Delete(iconFile);
                }

                int launcherindexNumber = int.Parse(launcherTileNumber) - 1;
                string launcherPath = LauncherPaths[launcherindexNumber];
                TextBox launcherTexbBox = LauncherTexBoxes[launcherindexNumber];

                if (launcherPath.Length > 1)
                {
                    launcherTexbBox.Text = "";
                }
            }
        }

        private void LauncherNameEnter(object sender, KeyEventArgs e)
        {
            if (sender is TextBox SenderTextBox)
            {
                launcherTileNumber = SenderTextBox.Tag.ToString();
            }

            string configName = "Launcher" + launcherTileNumber + "Name";
            string configValue;
            int launcherindexNumber = int.Parse(launcherTileNumber) - 1;
            string launcherPath = LauncherPaths[launcherindexNumber];
            TextBox launcherTexbBox = LauncherTexBoxes[launcherindexNumber];

            if (launcherPath.Length > 1)
            {
                configValue = launcherTexbBox.Text;
                ConfigManager.IniWrite(configName, configValue);
            }
        }

        private IEnumerable<Grid> NavigationGrids
        {
            get
            {
                List<Grid> navGrids = new List<Grid>
                {
                    GridMonitoring,
                    GridPerformance,
                    GridLauncher,
                    GridOSTweaks,
                    GridSettings,
                    GridAbout
                };
                return navGrids;
            }
        }

        private IEnumerable<Border> NavigationIndicators
        {
            get
            {
                List<Border> navIndicators = new List<Border>
                {
                    NavMonitoringIndicator,
                    NavPerformanceIndicator,
                    NavLauncherIndicator,
                    NavOSTweaksIndicator,
                    NavSettingsIndicator,
                    NavAboutIndicator
                };
                return navIndicators;
            }
        }

        private IEnumerable<Border> SettingsTabIndicators
        {
            get
            {
                List<Border> tabIndicators = new List<Border>
                {
                    SettingsTimerServiceindicator,
                    SettingsAudioDevicesindicator,
                    SettingsLauncherindicator,
                    SettingsThemeindicator
                };
                return tabIndicators;
            }
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

        private static IEnumerable<Guid> GetAll()
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

        private List<string> LauncherNames
        {
            get
            {
                List<string> launcherNames = new List<string>
                {
                    _launcher1Name,
                    _launcher2Name,
                    _launcher3Name,
                    _launcher4Name,
                    _launcher5Name,
                    _launcher6Name,
                    _launcher7Name,
                    _launcher8Name,
                    _launcher9Name,
                    _launcher10Name,
                    _launcher11Name,
                    _launcher12Name,
                    _launcher13Name,
                    _launcher14Name,
                    _launcher15Name,
                    _launcher16Name
                };
                return launcherNames;
            }
        }

        private List<string> LauncherPaths
        {
            get
            {
                List<string> launcherPaths = new List<string>
                {
                    _launcher1Path,
                    _launcher2Path,
                    _launcher3Path,
                    _launcher4Path,
                    _launcher5Path,
                    _launcher6Path,
                    _launcher7Path,
                    _launcher8Path,
                    _launcher9Path,
                    _launcher10Path,
                    _launcher11Path,
                    _launcher12Path,
                    _launcher13Path,
                    _launcher14Path,
                    _launcher15Path,
                    _launcher16Path
                };
                return launcherPaths;
            }
        }

        private List<Tile> LauncherTiles
        {
            get
            {
                List<Tile> launcherTiles = new List<Tile>
                {
                    LauncherTile1,
                    LauncherTile2,
                    LauncherTile3,
                    LauncherTile4,
                    LauncherTile5,
                    LauncherTile6,
                    LauncherTile7,
                    LauncherTile8,
                    LauncherTile9,
                    LauncherTile10,
                    LauncherTile11,
                    LauncherTile12,
                    LauncherTile13,
                    LauncherTile14,
                    LauncherTile15,
                    LauncherTile16
                };
                return launcherTiles;
            }
        }

        private List<Image> LauncherTileImages
        {
            get
            {
                List<Image> launcherTileImages = new List<Image>
                {
                    LauncherTile1Image,
                    LauncherTile2Image,
                    LauncherTile3Image,
                    LauncherTile4Image,
                    LauncherTile5Image,
                    LauncherTile6Image,
                    LauncherTile7Image,
                    LauncherTile8Image,
                    LauncherTile9Image,
                    LauncherTile10Image,
                    LauncherTile11Image,
                    LauncherTile12Image,
                    LauncherTile13Image,
                    LauncherTile14Image,
                    LauncherTile15Image,
                    LauncherTile16Image
                };
                return launcherTileImages;
            }
        }

        private List<TextBox> LauncherTexBoxes
        {
            get
            {
                List<TextBox> launcherTextBoxes = new List<TextBox>
                {
                    LauncherTextBox1,
                    LauncherTextBox2,
                    LauncherTextBox3,
                    LauncherTextBox4,
                    LauncherTextBox5,
                    LauncherTextBox6,
                    LauncherTextBox7,
                    LauncherTextBox8,
                    LauncherTextBox9,
                    LauncherTextBox10,
                    LauncherTextBox11,
                    LauncherTextBox12,
                    LauncherTextBox13,
                    LauncherTextBox14,
                    LauncherTextBox15,
                    LauncherTextBox16
                };
                return launcherTextBoxes;
            }
        }
    }
}