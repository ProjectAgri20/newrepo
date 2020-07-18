using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.Utility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TraceFactory.SetThreadContextProperty("ProcessId", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));
            Thread.CurrentThread.SetName("Main");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Load settings from App.Config
            GlobalSettings.Load();

            using (Form mainForm = new MainForm())
            {
                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                ApplicationExceptionHandler.Attach(mainForm);
                Application.Run(mainForm);
            }
        }
    }
}
