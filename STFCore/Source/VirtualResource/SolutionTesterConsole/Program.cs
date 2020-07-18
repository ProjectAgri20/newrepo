using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            TraceFactory.SetThreadContextProperty("ProcessId", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));

            string instanceId = string.Empty;
            string sessionId = string.Empty;

            if (args.Length == 2)
            {
                instanceId = args[0];
                sessionId = args[1];

                TraceFactory.SetThreadContextProperty("InstanceId", instanceId);
                TraceFactory.SetThreadContextProperty("SessionId", sessionId);

                GlobalSettings.IsDistributedSystem = false;

                Thread.CurrentThread.SetName("Main");
                TraceFactory.Logger.Debug("Username set to {0}".FormatWith(instanceId));
            }
            else
            {
                throw new ArgumentOutOfRangeException("Invalid arguments : {0}".FormatWith(string.Join(", ", args)));
            }

            TraceFactory.Logger.Debug("User: {0}".FormatWith(instanceId));
            TraceFactory.Logger.Debug("Starting...");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            WorkerExecutionForm mainForm = new WorkerExecutionForm("localhost", instanceId, LogFileReader.DataLogPath(sessionId));
            mainForm.WindowState = FormWindowState.Minimized;
            Application.Run(mainForm);
        }
    }
}
