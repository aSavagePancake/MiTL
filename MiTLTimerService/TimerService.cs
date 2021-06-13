using System.Runtime.InteropServices;
using System.ServiceProcess;

namespace TimerService
{
    public partial class TimerService : ServiceBase
    {
        public TimerService()
        {
            InitializeComponent();
            ServiceName = "MiTLTimerService";
            CanStop = true;
        }

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtSetTimerResolution(int desiredResolution, bool setResolution, out int currentResolution);

        [DllImport("ntdll.dll", SetLastError = true)]
        private static extern int NtQueryTimerResolution(out int minimumResolution, out int maximumResolution, out int actualResolution);

        private static int CurrentTimerRes()
        {
            NtQueryTimerResolution(out _, out _, out int currentResolution);
            return currentResolution;
        }

        private static int MaximumTimerRes()
        {
            NtQueryTimerResolution(out _, out int maximumResolution, out _);
            return maximumResolution;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            if (CurrentTimerRes().Equals(MaximumTimerRes())) return;
            int desiredResolution = MaximumTimerRes();
            NtSetTimerResolution(desiredResolution, true, out _);
        }

        protected override void OnStop()
        {
        }
    }
}