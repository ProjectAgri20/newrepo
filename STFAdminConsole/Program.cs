using System;
using System.Windows.Forms;
using HP.ScalableTest.UI.Framework;

namespace HP.ScalableTest.LabConsole
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (Form mainForm = new MainForm())
            {
                Environment.CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;
                ApplicationExceptionHandler.Attach(mainForm);
                Application.Run(mainForm);
            }
        }
    }
}