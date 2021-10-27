using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace MiTL
{
    internal static class ServiceManager
    {
        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetTimerResolution(int desiredResolution, bool setResolution, out int currentResolution);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryTimerResolution(out int minimumResolution, out int maximumResolution, out int actualResolution);

        private static readonly string ServicePath = Directory.GetCurrentDirectory() + @"\bin\TimerService.exe";
        public static readonly ServiceController MiTLService = new ServiceController("MiTLTimerService");

        public static bool ServiceExists()
        {
            return ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals("MiTLTimerService"));
        }

        public static bool ServiceRunning()
        {
            bool isRunning = false;
            switch (MiTLService.Status)
            {
                case ServiceControllerStatus.Running:
                    isRunning = true;
                    break;

                case ServiceControllerStatus.ContinuePending:
                    break;

                case ServiceControllerStatus.Paused:
                    break;

                case ServiceControllerStatus.PausePending:
                    break;

                case ServiceControllerStatus.StartPending:
                    break;

                case ServiceControllerStatus.Stopped:
                    break;

                case ServiceControllerStatus.StopPending:
                    break;

                default:
                    isRunning = false;
                    break;
            }
            return isRunning;
        }

        public static int CurrentTimerRes()
        {
            NtQueryTimerResolution(out _, out _, out int currentResolution);
            return currentResolution;
        }

        public static void InstallService()
        {
            if (ServiceExists())
            {
                return;
            }

            Process installServiceProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe",
                    Arguments = "\"" + ServicePath + "\"",
                    RedirectStandardError = false,
                    RedirectStandardOutput = false
                }
            };
            installServiceProcess.Start();
            installServiceProcess.WaitForExit(500);
        }

        public static void UninstallService()
        {
            if (!ServiceExists())
            {
                return;
            }

            StopService();
            Process uninstallServiceProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = Environment.GetFolderPath(Environment.SpecialFolder.Windows) + @"\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe",
                    Arguments = "/u \"" + ServicePath + "\"",
                    RedirectStandardError = false,
                    RedirectStandardOutput = false
                }
            };
            uninstallServiceProcess.Start();
            uninstallServiceProcess.WaitForExit(500);
        }

        public static void StartService()
        {
            if (!ServiceExists())
            {
                return;
            }

            if (!ServiceRunning())
            {
                MiTLService.Start();
            }
        }

        public static void StopService()
        {
            if (!ServiceExists())
            {
                return;
            }

            if (ServiceRunning())
            {
                MiTLService.Stop();
            }
        }

        //public static void StartStopService()
        //{
        //    if (!ServiceExists())
        //    {
        //        return;
        //    }

        //    if (!ServiceRunning())
        //    {
        //        MiTLService.Start();
        //    }

        //    if (ServiceRunning())
        //    {
        //        MiTLService.Stop();
        //    }
        //}
    }
}