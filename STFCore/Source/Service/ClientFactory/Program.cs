using System;
using System.Globalization;
using System.IO;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Utility;  

namespace HP.ScalableTest.Service.ClientFactory
{
    /// <summary>
    /// Main program for this service.
    /// </summary>
    public static class Program
    {
        private static void Main(string[] args)
        {
            UnhandledExceptionHandler.Attach();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Thread.CurrentThread.SetName("Main");

            VirtualClientController clientController = null;
            try
            {
                TraceFactory.Logger.Debug("Args: {0}".FormatWith(string.Join(" ", args)));
                int startIndex = 0;
                string sessionId = string.Empty;
                switch (args.Length)
                {
                    case 2:
                        sessionId = args[1];
                        break;
                    case 3:
                        sessionId = args[1];
                        startIndex = int.Parse(args[2], CultureInfo.InvariantCulture);
                        break;
                    default:
                        TraceFactory.Logger.Fatal("Invalid number of arguments.  Arguments passed in: {0}".FormatWith(args.Length));
                        TraceFactory.Logger.Fatal("Usage: {0} <Dispatcher Fully Qualified Domain Name> <Session Id> <Start Index for Software Installer>".FormatWith(AppDomain.CurrentDomain.FriendlyName));
                        Environment.Exit(1);
                        break;
                }

                // Set the logging context 
                var dispatcher = args[0].Split('.')[0];
                TraceFactory.SetSessionContext(sessionId);
                TraceFactory.SetThreadContextProperty("Dispatcher", dispatcher, false);
                using (var officeWorkerStream =
                    File.Create(Path.Combine(Environment.CurrentDirectory, "OfficeWorkerBootStrapper.exe")))
                {
                    officeWorkerStream.Write(Properties.Resources.OfficeWorkerBootStrapper, 0,
                        Properties.Resources.OfficeWorkerBootStrapper.Length);
                    officeWorkerStream.Flush(true);
                }

                
                clientController = new VirtualClientController(dispatcher, startIndex);
                clientController.Start(sessionId);
                TraceFactory.Logger.Debug("Client controller started.  Press Enter to terminate.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            TraceFactory.Logger.Debug("Exiting...");
        }
    }
}