
namespace HP.ScalableTest.Service.PrintMonitor
{
    static class Program
    {
        static void Main(string[] args)
        {
            // Initialize the PrintMonitorService, so that it will begin monitoring print queues immediately.
            PrintMonitorService.Initialize();

            using (PrintMonitorWindowsService service = new PrintMonitorWindowsService())
            {
                service.Run(args);
            }
        }
    }
}