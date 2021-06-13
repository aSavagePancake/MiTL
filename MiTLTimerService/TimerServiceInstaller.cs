using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace TimerService
{
    [RunInstaller(true)]
    public partial class TimerServiceInstaller : Installer
    {
        public TimerServiceInstaller()
        {
            InitializeComponent();
            serviceInstaller.Description = "MiTL Timer Service";
            serviceInstaller.DisplayName = "MiTL Timer Service";
            serviceInstaller.ServiceName = "MiTLTimerService";
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
        }
    }
}