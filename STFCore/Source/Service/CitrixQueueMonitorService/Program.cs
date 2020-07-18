
namespace HP.ScalableTest.Service.Citrix
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (CitrixQueueMonitorWindowsService service = new CitrixQueueMonitorWindowsService())
            {
                service.Run(args);
            }
        }
    }
}