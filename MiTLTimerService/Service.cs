using System.ServiceProcess;

namespace TimerService
{
    internal static class Service
    {
        private static void Main()
        {
            TimerService service = new TimerService();
            ServiceBase.Run(service);
        }
    }
}