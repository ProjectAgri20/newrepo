using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.  Note that the arguments will come
        /// into this Main as comma separated.  This is to support the ICA launch
        /// of this application on Citrix.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            TraceFactory.SetThreadContextProperty("ProcessId", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));

            string controllerHost = string.Empty;
            string instanceId = string.Empty;

            if (args.Length == 0)
            {
                // This is for handling Citrix Worker
                Thread.CurrentThread.SetName("Main");

                string datFile = @"C:\VirtualResource\UserConfiguration\{0}.dat".FormatWith(Environment.UserName);

                if (File.Exists(datFile))
                {
                    var items = File.ReadAllText(datFile).Split(',');
                    controllerHost = items[0];
                    instanceId = items[1];
                    TraceFactory.SetThreadContextProperty("InstanceId", instanceId);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("Local data file does not exist for user {0}".FormatWith(instanceId));
                }
            }
            else if (args.Length == 2)
            {
                // This is for handling the Office Worker and Admin Worker
                Thread.CurrentThread.SetName("Main");

                controllerHost = args[0];
                instanceId = args[1];
                TraceFactory.SetThreadContextProperty("InstanceId", instanceId);
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid arguments : {0}".FormatWith(string.Join(", ", args)));
            }

            TraceFactory.Logger.Debug("Host: {0}, InstanceId: {1}".FormatWith(controllerHost, instanceId));
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WorkerExecutionForm(controllerHost, instanceId, LogFileReader.DataLogPath()));
        }
    }    
}
