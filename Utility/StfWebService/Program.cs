using System;

namespace HP.ScalableTest.Service.StfWebService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main(string[] args)
        {
            //RunDebugMode();
            RunServiceMode(args);
        }

        private static void RunDebugMode()
        {
            Console.WriteLine("Starting Web Api Server...");

            var service = new StfWebApiService();

            service.RunDebug(new[] { "localhost" });
            Console.ReadKey();
        }

        public static void RunServiceMode(string[] args)
        {
            using (var stfWebService = new StfWebApiService())
            {
                stfWebService.Run(args);
            }
        }
    }
}