using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MiTL
{
    internal class ProcessManager
    {
        private const int Delay = 500;

        public static void TerminateDuplicate()
        {
            if (Process.GetProcessesByName(Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()?.Location)).Length > 1)
            {
                Process.GetCurrentProcess().Kill();
            }
        }

        public static void TerminateApp(string processName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Contains(processName))
                {
                    process.Kill();
                }
            }
        }

        public static void CloseAfterProcessStart(string processName)
        {
            Task.Factory.StartNew(function: async () =>
            {
                while (true)
                {
                    if (IsProcessRunning(processName))
                    {
                        System.Environment.Exit(0);
                    }
                    await Task.Delay(Delay).ConfigureAwait(false);
                }
            });
        }

        private static bool IsProcessRunning(string processName)
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.Contains(processName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}