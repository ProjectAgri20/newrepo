namespace DartLogCollectorService
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            using (DartLogWindowsCollector service = new DartLogWindowsCollector())
            {
                service.Run(args);
            }
        }
    }
}